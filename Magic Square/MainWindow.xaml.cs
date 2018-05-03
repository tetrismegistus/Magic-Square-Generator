using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using CsvHelper;
using Microsoft.Win32;

namespace Magic_Square
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

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            int base_input = 0;
            List<List<string>> square;

            if (Int32.TryParse(baseInput.Text, out base_input))
            {
                square = GetMagicSquare(base_input);
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog.Filter = "Comma-Delimited Files (*.csv) |*.csv";
                if (saveFileDialog.ShowDialog() == true)
                    WriteToCsv(saveFileDialog.FileName, square);                
                
            }
        }

        private void WriteToCsv(string filename, List<List<string>> data)
        {
            using (TextWriter writer = new StreamWriter(filename))
            {
                var csv = new CsvWriter(writer);
                foreach (var row in data)
                {
                    foreach (var value in row)
                    {
                        csv.WriteField(value);
                    }
                    csv.NextRecord();
                    
                }
                MessageBox.Show("File written!", "Great Success");
                writer.Flush();                
            }
        }
        
        private string NumToLetter(int integer)
        {
            if ((10 <= integer) && (integer <= 35))
            {
                return Convert.ToString(Convert.ToChar(integer + 55));
            }
            else if ((36 <= integer) && (integer <= 61))
            {
                return Convert.ToString(Convert.ToChar(integer + 61));
            }
            else
            {
                return Convert.ToString(integer);
            };                      
        }

        private List<List<string>> GetMagicSquare(int base_integer)
        {
            int max_digit = base_integer - 1;
            List<int> left_column = Fib(1, max_digit);
            int reference_length = left_column.Count();
            List<List<string>> square = new List<List<string>>();

            foreach (int x in left_column)
            {
                // in english, I mean python
                // row = [num_to_letter(i) for i in fib(x, max_digit)]                                
                List<string> row = new List<string>();
                foreach (int number in Fib(x, max_digit))
                {
                    row.Add(NumToLetter(number));
                }

                int row_length = row.Count();
                if (row_length != reference_length)
                {
                    List<string> row_copy = new List<string>(row);
                    for (int i = 0; i < (reference_length / row_length) - 1; i++)
                    {
                        row.AddRange(row_copy);
                    }
                }
                square.Add(row);
            }
            return square;
        }

        private List<int> Fib(int start, int max_digit)
        {
            int a;      
            int b;                  
            int seqLen;
            int eval;

            List<int> seq = new List<int>();

            if (start == 1)
            {
                a = b = start;                
            }
            else
            {
                a = b = ((start % max_digit) > 0) ? start % max_digit : max_digit;
            }


            while (true)
            {
                seq.Add(a);
                eval = (((a + b) % max_digit) > 0) ? (a + b) % max_digit : max_digit;
                a = b;                                
                b = eval;
                seqLen = seq.Count();
                if (seqLen > 3)
                {
                    List<int> seq_beginning = seq.GetRange(0, 3);
                    List<int> seq_end = seq.GetRange(seqLen - 3, 3);
                    if (seq_beginning.SequenceEqual(seq_end))
                    {
                        return seq.GetRange(0, seqLen - 3);
                    }
                }
                

            }
        }
    }
}
