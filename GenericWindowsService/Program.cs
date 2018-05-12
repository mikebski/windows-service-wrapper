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
using System.ServiceProcess;
using System.Text;

namespace GenericWindowsService
{
    static class Program
    {
        /// <summary>
        /// This method starts the service.
        /// </summary>
        static void Main()
        {
            // To run more than one service you have to add them here
            ServiceBase.Run(new ServiceBase[] { new GenericWindowsService() });
        }
    }
}
