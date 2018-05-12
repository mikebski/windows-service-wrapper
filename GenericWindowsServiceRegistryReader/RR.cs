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
using Microsoft.Win32;
using System.Collections.Generic;

namespace GenericWindowsServiceRegistryReader
{
    public class RR
    {
        public const String BASE_KEY = @"SOFTWARE\Wow6432Node\mikeski.net\GenericWindowsService";
        private RegistryKey baseKey;
        private IDictionary<String, String> configurations;
        private String configFileName;
        
        public RR(){
            configurations = new Dictionary<String, String>();
            baseKey = Registry.LocalMachine.OpenSubKey(BASE_KEY, false);
            if(baseKey == null){ return; }
            
            var sks = baseKey.GetSubKeyNames();
            foreach(var k in sks){
                var sk = baseKey.OpenSubKey(k);
                configurations.Add(k, sk.GetValue("ConfigurationFile").ToString());
                configFileName = sk.GetValue("ConfigurationFile").ToString();
            }
        }

        public IDictionary<String, String> GetConfigurations(){
            return configurations;
        }
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Reading registry");
            Console.WriteLine("Base key is " + RR.BASE_KEY);
            var rr = new RR();
            Console.WriteLine(rr.GetConfigurations());
            foreach(var k in rr.GetConfigurations().Keys){
                Console.WriteLine("Key is [" + k + "]");
                Console.WriteLine("Value is [" + rr.GetConfigurations()[k] + "]");
            }
            Console.Write("Press any key to continue . . . ");
            
            Console.ReadKey(true);
        }
    }
}