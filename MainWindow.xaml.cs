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
        private int counter = 0;
        private readonly object syncLock = new();
        private List<int> results = new();
        private const int NumIteration = 10;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void RaceData_Click(object sender, RoutedEventArgs e)
        {
            raceResult.Content = "";
            results.Clear();
            Thread thread1 = new(() => IncrementCounter());
            Thread thread2 = new(() => IncrementCounter());
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            if (counter != 20)
            {
                raceResult.Content = $"Ошибка: значение должно быть 20, а стало  {counter}";
            }
               
            
               
        }

        private void IncrementCounter()
        {
            for (int i = 0; i < NumIteration; i++) 
            {
                var temp = counter + 1;
                Thread.Sleep(0);
                counter = temp;
            }
        }

        private void SafeAdd_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            safeResult.Content = "";
            results.Clear();
            Thread thread1 = new(() => LockedIncrementCounter());
            Thread thread2 = new(() => LockedIncrementCounter());
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            if (counter != 20)
            {
                safeResult.Content = $"Ошибка: значение должно было стать равно 20, но стало {counter}";
            }
                
            else
            {
                safeResult.Content = "Все отлично!";
            }
              
        }

        private void LockedIncrementCounter()
        {
            for (int i = 0; i < NumIteration; i++) 
            {
                lock (syncLock)
                    ++counter;
            }
        }

        private void MonitorTimeout_Click(object sender, RoutedEventArgs e)
        {
            monitorResult.Content = "";
            bool entered = false;
            try
            {
                entered = Monitor.TryEnter(syncLock, TimeSpan.FromMilliseconds(10));
                if (!entered)
                    monitorResult.Content = "Монитор занят другим потоком.";
                else
                    monitorResult.Content = " успешно";
            }
            finally
            {
                if (entered)
                 Monitor.Exit(syncLock);
            }
        }

    }
}





