/*
 *  Copyright 2018 Mike Baranski (mike.baranski@gmail.com)
 *  
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *  
 *      http://www.apache.org/licenses/LICENSE-2.0
 *  
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using RunProcess;
using GenericWindowsServiceRegistryReader;

namespace GenericWindowsService
{
    public class GenericWindowsService : ServiceBase
    {
        string path = @"c:\temp\svclog.txt";
        StreamWriter sw;
        public const String MyServiceName = "GenericWindowsService";
        private IDictionary<String, String> registryConfiguration;
        IList<HeadlessProgram> programs;
        private HeadlessProgramConfiguration programConfiguration;
        
        public GenericWindowsService()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            if (!File.Exists(path)) {
                // Create a file to write to. 
                sw = File.CreateText(path);
                sw.WriteLine("Created log file");
            }
            else { 
                sw = File.AppendText(path);
            }
            sw.AutoFlush = true;
            sw.WriteLine("Starting service...");
             
            programs = new List<HeadlessProgram>();
            this.ServiceName = MyServiceName;
            var rr = new RR();
            
            registryConfiguration = rr.GetConfigurations();
            
            var configFilename = "";
            foreach (var k in registryConfiguration.Keys) {
                sw.WriteLine("Loading config for " + k);
                configFilename = registryConfiguration[k];
                programConfiguration = HeadlessProgramConfiguration.FromXML(System.IO.File.ReadAllText(configFilename));
                programs.Add(new HeadlessProgram(programConfiguration));
            }
        }
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            // TODO: Add cleanup code here (if required)
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Start this service.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            sw.WriteLine("Starting programs");
            foreach (HeadlessProgram p in programs) {
                try{
                sw.WriteLine("Starting " + p.configuration.proc);
                p.RunProgram(sw);
                sw.WriteLine("Started");
                } catch (Exception e){
                    sw.WriteLine("Exception: " + e.Message);
                }
            }
            sw.WriteLine("Finished " + programs.Count);
        }
        
        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            foreach (var p in programs) {
                p.Terminate();
            }
            sw.Close();
        }
    }
}
