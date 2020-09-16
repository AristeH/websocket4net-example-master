
using System;
using System.Windows;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using WebSocket4Net;
using websocket4net_example;
using System.Windows.Threading;
using System.Windows.Forms;

namespace websocket4net_example
{
    /// <summary>
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>

    public partial class Window2 : Window
    {

       internal struct controls
        {
            public string control;
            public string typevalue;
            public string name;
            public string value; 
 
            public controls(string name_, string typevalue_, string control_, string value_)
            {
                name      = name_;
                typevalue = typevalue_;
                control   = control_;
                value     = value_;
            }

        }

        public WebSocket websocket;

        public string Text { get; set; }

        public Window2()
        {
            InitializeComponent();
            this.Width = 500;
        }
        internal void LoadOrders(String CustomerID)
        {

            var sr = new StringReader(CustomerID);
            int count = 0;
            string line;

            List<List<controls>> list = new List<List<controls>>();
            // читаем строку
            while ((line = sr.ReadLine()) != null)
            {
                count++;
                Console.WriteLine("Line {0}: {1}", count, line);

                // разбили на поля 
                using (CsvReader csv = new CsvReader(new StringReader(line), false, ';'))
                {

                    int ContrlCount = csv.FieldCount; // количество строк для генерации на форме
                    
                    // читаем поле
                    while (csv.ReadNextRecord())
                    {
                        //добавил строку в список
                    
                        for (int i = 0; i < ContrlCount; i++)
                        {
                            // разбиваем строку на строки элементов интерфейса
                            using (CsvReader csv1 = new CsvReader(new StringReader(csv[i]), false, ','))
                            {
                                
                                int fieldCount1 = csv1.FieldCount;
                                if (count == 1)
                                {
                                    // добавляем новую строку в интерфейс
                                    list.Add(new List<controls>());
                                }
                                    while (csv1.ReadNextRecord())
                                {

                                    for (int i1 = 0; i1 < fieldCount1; i1++)
                                    {
                                        if (count == 1) {
                                            controls contrl = new controls("", "", "", "");
                                            contrl.name = csv1[i1];
                                            list[i].Add(contrl);
                                            //  contrl[numfield] = ;
                                        }
                                        if (count == 2) {
                                            controls kk = list[i][i1];
                                                kk.typevalue = csv1[i1];
                                            list[i][i1] = kk;
                                        }
                                        if (count == 3) {
                                            controls kk = list[i][i1];
                                            kk.control = csv1[i1];
                                            list[i][i1] = kk;
                                        }


                                    }
                                        
                                    }
                                   

                                }
                                
                            }
                         
                       
                    } //while  прочитали файл
                   


                        }
                        
                    }

            generateinterface(list);
        }

        internal void generateinterface(List<List<controls>>list)

        {
            int count = list.Count;

            StackPanel spv = new StackPanel();
            this.GR.Children.Add(spv);
           
            //обход строк
            for (int i = 0; i < count; i++)
            {
                StackPanel spg = new StackPanel();
               spg.Orientation = System.Windows.Controls.Orientation.Horizontal;
                spv.Children.Add(spg);
               
                int stolbec = list[i].Count;

                Grid gr = new Grid();
                gr.Width = 460;
                for (int j = 0; j < stolbec; j++)
                {
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    gr.ColumnDefinitions.Add(colDef1);
                }
                    
                RowDefinition rowDef1 = new RowDefinition(); 
                gr.RowDefinitions.Add(rowDef1); 

                spg.Children.Add(gr);
                // обход элементов
                for (int j = 0; j < stolbec; j++)
                {
                    string rez = list[i][j].typevalue;
                    
                    switch (rez)
                    {
                        case "5":
                            System.Windows.Controls.Label tb = new System.Windows.Controls.Label { Content = list[i][j].name };
                            tb.Margin = new System.Windows.Thickness(5, 5, 5, 5);
                            Grid.SetRow(tb, 0);
                            Grid.SetColumn(tb,j);
                            gr.Children.Add(tb);
                           // gr.Children.Add(new Label { Content = list[i][j].name, Background = new SolidColorBrush(System.Windows.Media.Colors.Orange) });
                            break;
                        case "7":
                            System.Windows.Controls.TextBox tb1 = new System.Windows.Controls.TextBox { Text = list[i][j].name, VerticalAlignment = VerticalAlignment.Center };
                            tb1.Margin = new System.Windows.Thickness(5, 5, 5, 5);
                            Grid.SetRow(tb1, 0);
                            Grid.SetColumn(tb1, j);
                            gr.Children.Add(tb1);
                           // spg.Children.Add(new TextBox { Text = list[i][j].name, VerticalAlignment = VerticalAlignment.Center });
                            Console.WriteLine("edit");
                            break;
                        case "9":
                            PasswordBox pb = new PasswordBox {Name="pb", DataContext = list[i][j].name };
                            pb.Margin = new System.Windows.Thickness(5, 5, 5, 5);
                            Grid.SetRow(pb, 0);
                            Grid.SetColumn(pb, j);
                            gr.Children.Add(pb);
                            // gr.Children.Add(new Label { Content = list[i][j].name, Background = new SolidColorBrush(System.Windows.Media.Colors.Orange) });
                            break;
                        case "1":
                            System.Windows.Controls.Button bt = new System.Windows.Controls.Button { Name = list[i][j].name, Content = list[i][j].name };
                            Grid.SetRow(bt, 0);
                            Grid.SetColumn(bt, j);
                            bt.Width = 100;
                            bt.Height = 25;
                            bt.Margin = new System.Windows.Thickness(5, 5, 5, 5); 
                            bt.Background = new SolidColorBrush(System.Windows.Media.Colors.Orange);
                            bt.Click += new RoutedEventHandler(button_Click);
                            bt.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
                            gr.Children.Add(bt);
                            // gr.Children.Add(new Label { Content = list[i][j].name, Background = new SolidColorBrush(System.Windows.Media.Colors.Orange) });
                            break;
                    }

                }
            }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                // this.FindName("pb") 
                string name = (sender as System.Windows.Controls.Button).Name;
                string n = this.Name;
                websocket.Send(name);

            }));
        }

    }


}
   




                


