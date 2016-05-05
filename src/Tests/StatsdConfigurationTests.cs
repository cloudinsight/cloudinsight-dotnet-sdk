using System;
using System.Threading;
using NUnit.Framework;
using StatsdClient;
using Tests.Helpers;


namespace Tests
{
    [TestFixture]
    public class StatsdConfigurationTest
    {
        private void testReceive(string testServerName, int testPort, string testCounterName,
                                 string expectedOutput)
        {
            UdpListener udpListener = new UdpListener(testServerName, testPort);
            Thread listenThread = new Thread(new ParameterizedThreadStart(udpListener.Listen));
            listenThread.Start();
            CloudInsightStatsd.Increment(testCounterName);
            while (listenThread.IsAlive) ;
            Assert.AreEqual(expectedOutput, udpListener.GetAndClearLastMessages()[0]);
            udpListener.Dispose();
        }

        [Test]
        public void throw_exception_when_no_config_provided()
        {
            StatsdConfig metricsConfig = null;
            Assert.Throws<ArgumentNullException>(() => StatsdClient.CloudInsightStatsd.Configure(metricsConfig));
        }

        [Test]
        public void throw_exception_when_no_hostname_provided()
        {
            var metricsConfig = new StatsdConfig { };
            Assert.Throws<ArgumentNullException>(() => StatsdClient.CloudInsightStatsd.Configure(metricsConfig));
        }

        [Test]
        public void default_port_is_8251()
        {
            var metricsConfig = new StatsdConfig
            {
                StatsdServerName = "127.0.0.1"
            };
            StatsdClient.CloudInsightStatsd.Configure(metricsConfig);
            testReceive("127.0.0.1", 8251, "test", "test:1|c");
        }

        [Test]
        public void setting_port()
        {
            var metricsConfig = new StatsdConfig
            {
                StatsdServerName = "127.0.0.1",
                StatsdPort = 8252
            };
            StatsdClient.CloudInsightStatsd.Configure(metricsConfig);
            testReceive("127.0.0.1", 8252, "test", "test:1|c");
        }

        [Test]
        public void setting_prefix()
        {
            var metricsConfig = new StatsdConfig
            {
                StatsdServerName = "127.0.0.1",
                StatsdPort = 8253,
                Prefix = "prefix"
            };
            StatsdClient.CloudInsightStatsd.Configure(metricsConfig);
            testReceive("127.0.0.1", 8253, "test", "prefix.test:1|c");
        }
    }
}
