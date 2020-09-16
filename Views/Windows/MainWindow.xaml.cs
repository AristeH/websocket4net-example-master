using LumenWorks.Framework.IO.Csv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using System.Xaml;
using System.Xml;
using WebSocket4Net;
using websocket4net_example;

namespace websocket_example_window
{


   

    struct Mess
    {
        public string action;
        public string name;
        public string[] parameters;
    }
    /// <summary>
    /// MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : Window
    {
        public struct ListForm
        {
            public string name;
            public string id;
        }

        public struct ListFormXaml
        {
            public string name;
            public string textxaml;
        }

        public List<ListForm> lf = new List<ListForm>(10);
        public List<ListFormXaml> lfx = new List<ListFormXaml>(10);

        //строка подключения к серверу
        private string serverURL = "ws://127.0.0.1:8080/telephon";
        private WebSocket websocket;
        private bool is_connected = false;
        public MainWindow()
        {
            InitializeComponent();
            initWebSocketClient();
        }

        private void initWebSocketClient()
        {
            if (serverURL == "")
            {
                textBox2.Text += "Не задана строка подключения\n";
            }
            else
            {
                websocket = new WebSocket(serverURL);
                websocket.Closed += new EventHandler(websocket_Closed);
                websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
                websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
                websocket.Opened += new EventHandler(websocket_Opened);
                textBox2.Text += "[init]\n";
                textBox1.Focus();
                websocket.Open();
            }
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            is_connected = true;

            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {

                button1.IsEnabled = true;
                textBox2.Text += "[Подключились]\n";



            }));

        }

        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {

            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {

                textBox2.Text += "[ошибка] " + e.Exception.Message + "\n";
                button1.IsEnabled = false;
            }));
        }

        private void websocket_Closed(object sender, EventArgs e)
        {

            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {

                textBox2.Text += "[отключились]\n";
                button1.IsEnabled = false;
            }));
        }

        // Получено сообщение необходимо обаработать его

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
             string action;
             string name;
             string text;
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                textBox2.Text += e.Message + "\n";
                textBox2.ScrollToEnd();
                using (CsvReader csv = new CsvReader(new StringReader(e.Message), false, ';'))
                {

                    int ContrlCount = csv.FieldCount; // количество строк для генерации на форме
                    action = "";
                    int i = 0;
                        while (csv.ReadNextRecord())
                    {
                        switch (i)
                        {
                            case 0:
                                action = csv[0];
                                break;
                            case 1:
                                name = csv[0];
                                break;
                            case 2:
                                text = csv[0];
                                break;
                        }
                        i++;
                    }
               

                    switch (action)
                    {
                        case "result":
                            Console.WriteLine("Case 1");
                            break;
                        case "form":
                            Console.WriteLine("Case 2");
                            break;
                        case "data":
                            Console.WriteLine("Case 2");
                            break;

                        default:
                            Console.WriteLine("Default case");
                            break;
                    }

                 //   StringReader stringReader = new StringReader(text);
                //    XmlReader xmlReader = XmlReader.Create(stringReader);

                 //   UIElement tree = (UIElement)XamlReader.Load(xmlReader);
               
                    Window2 subWindow = new Window2();
                    subWindow.Text = e.Message;
                    subWindow.websocket = websocket;
      //              this.LoadFromXaml(text); ;
                    subWindow.Show();
                }
            }));

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            sendMessage("login");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            initWebSocketClient();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sendMessage("login");
            }
        }

        private void sendMessage(string txt)
        {

            if (is_connected)
            {
                Mess tom = new Mess();
                tom.action = "getform";
                string[] arr = { "login", "login" };
                tom.parameters = arr;
                string json = JsonConvert.SerializeObject(tom);

                this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {

                    websocket.Send(json);
                    textBox2.Text += "I said \"" + textBox1.Text + "\"\n";
                    textBox1.Text = "";
                }));
            }
        }
    }

}

