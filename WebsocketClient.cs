using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using websocket4net_example;

namespace websocket4net_example
{
    class WebsocketClient
    {

        
        public WebSocket websocket;
        private bool is_connected = false;
        struct mess
        {
            public string action;
            public string[] parameters;
        }

        public void initWebSocketClient(String serverURL)
        {
            if (serverURL == "")
            {
             //   textBox2.Text += "Не задана строка подключения\n";
            }
            else
            {
                websocket = new WebSocket(serverURL);
                websocket.Closed += new EventHandler(websocket_Closed);
                websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
                websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
                websocket.Opened += new EventHandler(websocket_Opened);
           //     textBox2.Text += "[init]\n";
            //    textBox1.Focus();
                websocket.Open();
            }
        }

        public void websocket_Opened(object sender, EventArgs e)
        {
            is_connected = true;

          //  this.Invoke(DispatcherPriority.Background, new Action(() =>
          //  {

          //      button1.IsEnabled = true;
         //       textBox2.Text += "[Подключились]\n";



         //   }));

        }

        public void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {

           // this.Invoke(DispatcherPriority.Background, new Action(() =>
        //    {

        //        textBox2.Text += "[ошибка] " + e.Exception.Message + "\n";
        //        button1.IsEnabled = false;
        //    }));
        }

        public void websocket_Closed(object sender, EventArgs e)
        {

       //     this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
         //   {

          //      textBox2.Text += "[отключились]\n";
        //        button1.IsEnabled = false;
         //   }));
        }

        // Получено сообщение необходимо обаработать его

        public void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {

         //   this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
         //   {
         //       textBox2.Text += e.Message + "\n";
         //       textBox2.ScrollToEnd();
                //m.parameters[0] = e.Message;
                Window2 subWindow = new Window2();
                subWindow.Text = e.Message;

                subWindow.LoadOrders(e.Message);
                subWindow.Show();
        //    }));

        }

        public void sendMessage()
        {

            if (is_connected)
            {
                mess tom = new mess();
                tom.action = "get";
                string[] arr = { "form:list", "object:login" };
                tom.parameters = arr;
               // string json = JsonConvert.SerializeObject(tom);

            //    this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
             //   {

                    websocket.Send("login");
              //      textBox2.Text += "I said \"" + textBox1.Text + "\"\n";
             //       textBox1.Text = "";
             //   }));
            }
        }
    }
}

