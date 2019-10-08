using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Messaging;
using System.Xml;
using System.Xml.Linq;

namespace WriteToMSMQClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageQueue msgQ = new MessageQueue(txtQueuePath.Text);
                msgQ.Formatter = new XmlMessageFormatter(new Type[]{typeof(XmlDocument)});
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(txtMessage.Text);
                Message m1 = new Message();
                m1.Body = xml;
                msgQ.Send(m1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error sending to queue");                
            }
        }
    }
}
