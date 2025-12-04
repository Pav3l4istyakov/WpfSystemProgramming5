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

namespace WpfSystemProgramming5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int SharedCounter = 0;
        private readonly object SyncObj = new object();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void IncrementWithoutLock()
        {
            int temp = SharedCounter;
            temp++;  
            SharedCounter = temp;
        }
        private void ButtonRaceCondition_Click(object sender, RoutedEventArgs e)
        {
            SharedCounter = 0;
            var threads = new Thread[10]; 

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>  
                {
                    for (int j = 0; j < 10; j++)
                        IncrementWithoutLock(); 
                });
                threads[i].Start(); 
            }

            foreach (var count in threads)
             count.Join();

            MessageBox.Show($"Итоговый счетчик: {SharedCounter}", "Результат"); 
        }

      
        private void IncrementWithLock()
        {
            lock (SyncObj)
            {
            int temp = SharedCounter;
            temp++;
            SharedCounter = temp;
        }
        }
        private void ButtonSafeAdd_Click(object sender, RoutedEventArgs e)
        {
            SharedCounter = 0; 

            var threads = new Thread[10];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 10; j++)
                        IncrementWithLock(); 
                });
                threads[i].Start();
            }

            foreach (var t in threads)
                t.Join();

            MessageBox.Show($"Итоговый счетчик: {SharedCounter}", "Результат");
        }

     
        private void IncrementWithLock()
        {
            lock (SyncObj)
            {
                int temp = SharedCounter;
                temp++;
                SharedCounter = temp;
            }
        }

     
        private void ButtonMonitorTimeout_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;
            try
            {
                if (Monitor.TryEnter(SyncObj, TimeSpan.FromMilliseconds(10))) 
                {
                    success = true;
                    SharedCounter++;
                    Monitor.Exit(SyncObj);
                }
                else
                {
                    MessageBox.Show("Не удалось захватить блокировку.", "Ошибка");
                }
            }
            catch (SynchronizationLockException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка синхронизации");
            }

            if (success)
                MessageBox.Show($"Счетчик увеличен до {SharedCounter}.", "Результат");
        }
    }

}





