/*
 * Created by SharpDevelop.
 * User: mike.baranski
 * Date: 5/19/2015
 * Time: 8:58 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Security;

namespace RunProcess
{
    public class HeadlessProgramConfiguration
    {
        public String proc = @"d:\LightTPD\LightTPD.exe";
        public String args = @"-D  -f conf\lighttpd.conf";
        public String pwd = @"D:\LightTPD";
        public String user = null;
        public String pass = null;
        public String dom = null;
        
        public HeadlessProgramConfiguration()
        {
            user = null;
            pass = null;
            dom = null;
        }
		
        public HeadlessProgramConfiguration(String process, String arguments)
            : this()
        {
            proc = process;
            args = arguments;
        }
		
        public HeadlessProgramConfiguration(String process, String arguments, String username, String password, String domain)
            : this(process, arguments)
        {
            this.user = username;
            this.pass = password;
            this.dom = domain;
        }

        public HeadlessProgramConfiguration(String process, String arguments, String username, String password, String domain, String wd)
            : this(process, arguments, username, password, domain)
        {
            this.user = username;
            this.pass = password;
            this.dom = domain;
            this.pwd = wd;
        }
        
        public SecureString getSecurePassword()
        {
            var tp = new SecureString();
            if (pass == null) {
                tp = null;
            } else {
                foreach (var c in pass.ToCharArray())
                    tp.AppendChar(c);
            }
            return tp;
        }
        
        public String GetXML()
        {
            var s = new System.Xml.Serialization.XmlSerializer(typeof(HeadlessProgramConfiguration));
            var writer = new System.IO.StringWriter();
            s.Serialize(writer, this);
            return writer.ToString();
        }
        
        public static HeadlessProgramConfiguration FromXML(String xml)
        {
            var s = new System.Xml.Serialization.XmlSerializer(typeof(HeadlessProgramConfiguration));
            return (HeadlessProgramConfiguration)s.Deserialize(new System.IO.StringReader(xml));
        }
    }
    
    public class HeadlessProgram
    {
        private Process p;
        public HeadlessProgramConfiguration configuration;

        public HeadlessProgram(HeadlessProgramConfiguration c)
        {
            this.configuration = c;
        }
        
        public void RunProgram(System.IO.TextWriter sw)
        {
            if(sw == null){
                sw = Console.Out;
            }
            
            var psi = new ProcessStartInfo();
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = true;
            psi.CreateNoWindow = true;
            
            if (configuration.pwd != null) {
                if (sw != null) {
                    sw.WriteLine("Setting WorkingDirectory to " + configuration.pwd);
                }
                sw.WriteLine("Setting WorkingDirectory to " + configuration.pwd);
                Environment.CurrentDirectory = configuration.pwd;
                sw.WriteLine("Current directory is " + Environment.CurrentDirectory);
                psi.WorkingDirectory = configuration.pwd;
            }
            p = new Process();
            p.StartInfo = psi;
            p = Process.Start(configuration.proc, configuration.args, configuration.user, configuration.getSecurePassword(), configuration.dom);
        }
		
        public void Terminate()
        {
            try {
                p.Kill();
                p.Close();
                p.Dispose();
            } catch (Exception e) {
                Console.Write("Exception: " + e.Message);
            }
        }
		
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting command");
			
            var c = new HeadlessProgramConfiguration();
            Console.WriteLine("Configuration XML is: " + c.GetXML());
                              
            var myp = new HeadlessProgram(new HeadlessProgramConfiguration());
            myp.RunProgram(null);
			
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
            myp.Terminate();
        }
    }
}