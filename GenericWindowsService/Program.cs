/*
 * Created by SharpDevelop.
 * User: mike.baranski
 * Date: 5/19/2015
 * Time: 10:37 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
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
