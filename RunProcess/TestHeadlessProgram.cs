/*
 * Created by SharpDevelop.
 * User: mike.baranski
 * Date: 5/19/2015
 * Time: 10:10 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Runtime.InteropServices;
using System.Security;
using NUnit.Framework;

namespace RunProcess
{
    [TestFixture]
    public class TestHeadlessProgramConfiguration
    {
        const String testCmd = "test command";
        const String testArg = "test arg";
        const String testUser = "tu";
        const String testDom = "td";
        const String testWd = "c:\temp";
        SecureString testPass = null;
        
        [Test]
        public void TestDefaultConstructor()
        {
            var hpc = new HeadlessProgramConfiguration();
            Assert.IsNotNull(hpc.proc);
            Assert.IsNotNull(hpc.pwd);
            Assert.IsNotNull(hpc.args);
            Assert.IsNull(hpc.user);
            Assert.IsNull(hpc.pass);
            Assert.IsNull(hpc.dom);
        }
        
        [Test]
        public void TestTwoArgConstructor()
        {
            var hpc = new HeadlessProgramConfiguration(testCmd, testArg);
            
            StringAssert.AreEqualIgnoringCase(testCmd, hpc.proc);
            StringAssert.AreEqualIgnoringCase(testArg, hpc.args);
            Assert.IsNull(hpc.user);
            Assert.IsNull(hpc.pass);
            Assert.IsNull(hpc.dom);
        }
        
        [Test]
        public void TestFiveArgConstructor()
        {
            testPass = new SecureString();
            foreach (var c in "asdfpass".ToCharArray())
                testPass.AppendChar(c);
            var hpc = new HeadlessProgramConfiguration(testCmd, testArg, testUser, "asdfpass", testDom);
            
            StringAssert.AreEqualIgnoringCase(testCmd, hpc.proc);
            StringAssert.AreEqualIgnoringCase(testArg, hpc.args);
            StringAssert.AreEqualIgnoringCase(testUser, hpc.user);
            StringAssert.AreEqualIgnoringCase(testDom, hpc.dom);
            Assert.IsTrue(IsEqualTo(testPass, hpc.getSecurePassword()));
        }
        
        [Test]
        public void TestSixArgConstructor()
        {
            testPass = new SecureString();
            foreach (var c in "asdfpass".ToCharArray())
                testPass.AppendChar(c);
            var hpc = new HeadlessProgramConfiguration(testCmd, testArg, testUser, "asdfpass", testDom, testWd);
            
            StringAssert.AreEqualIgnoringCase(testCmd, hpc.proc);
            StringAssert.AreEqualIgnoringCase(testArg, hpc.args);
            StringAssert.AreEqualIgnoringCase(testUser, hpc.user);
            StringAssert.AreEqualIgnoringCase(testDom, hpc.dom);
            StringAssert.AreEqualIgnoringCase(testWd, hpc.pwd);
            Assert.IsTrue(IsEqualTo(testPass, hpc.getSecurePassword()));
        }
        
        [Test]
        public void TestGetAndFromXML()
        {
            testPass = new SecureString();
            foreach (var c in "asdfpass".ToCharArray())
                testPass.AppendChar(c);
            var hpc = new HeadlessProgramConfiguration(testCmd, testArg, testUser, "asdfpass", testDom, testWd);
            String xml = hpc.GetXML();
            var hpc2 = HeadlessProgramConfiguration.FromXML(xml);
            
            StringAssert.AreEqualIgnoringCase(hpc2.proc, hpc.proc);
            StringAssert.AreEqualIgnoringCase(hpc2.args, hpc.args);
            StringAssert.AreEqualIgnoringCase(hpc2.user, hpc.user);
            StringAssert.AreEqualIgnoringCase(hpc2.dom, hpc.dom);
            StringAssert.AreEqualIgnoringCase(hpc2.pwd, hpc.pwd);
            
        }
        
        [Test]
        public void TestConfig()
        {
            var hpcfg = new HeadlessProgramConfiguration();
            hpcfg.proc = "d:\\php-5.6.8\\php-cgi.exe";
            hpcfg.args = "-b localhost:9000";
            hpcfg.user = null;
            hpcfg.pass = null;
            hpcfg.dom = null;
            
            var xml = hpcfg.GetXML();
            Assert.IsNotNull(xml);
            
            var descfg = HeadlessProgramConfiguration.FromXML(xml);
            System.IO.File.WriteAllText("C:\\temp\\config.xml", xml);
            StringAssert.AreEqualIgnoringCase(xml, descfg.GetXML());
        }
        
        private static bool IsEqualTo(SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2) {
                    for (int x = 0; x < length1; ++x) {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2)
                            return false;
                    }
                } else
                    return false;
                return true;
            } finally {
                if (bstr2 != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstr1);
            }
        }
    }
}
