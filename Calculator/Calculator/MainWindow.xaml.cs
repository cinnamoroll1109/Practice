using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Calculator
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();           
        }
        
        public Stack<string> st_Operand = new Stack<string>();
        public Stack<char> st_Operator = new Stack<char>();

        private bool ifOperator(char c)
        {
            if (c == '+') return true;
            else if (c == '-') return true;
            else if (c == '*') return true;
            else if (c == '/') return true;
            else return false;
        }

        private int Computation(string _op1, string _op2, char _operator)
        {
            int op1 = Convert.ToInt32(_op1);
            int op2 = Convert.ToInt32(_op2);

            if (_operator == '+') return op1 + op2;
            else if (_operator == '-') return op1 - op2;
            else if (_operator == '*') return op1 * op2;
            else return op1 / op2; //_operator == '/'
        }

        private void Traversal(string type, string text)
        {
            int l = 0;
            string _operand = ""; //找連續的數字
            string op1 = "", op2 = ""; //operand1 and operand2
            char temp = ' '; // temporary operator
            string op_new = ""; //merge operands and temporary operator
           
            //掃瞄一輪
            while (l < text.Length)
            {
                if (!ifOperator(text[l])) //if not an operator
                {
                    _operand = _operand + text[l]; //如果下一個字元也是數字則合併一起
                }
                else
                {
                    st_Operand.Push(_operand);
                    _operand = "";

                    if (!st_Operator.Any()) //if stack is empty
                    {
                        st_Operator.Push(text[l]);
                    }
                    else
                    {
                        if (st_Operator.Peek() == '*' || st_Operator.Peek() == '/')
                        {
                            temp = st_Operator.Pop(); //取出運算子
                            op2 = st_Operand.Pop();
                            op1 = st_Operand.Pop();

                            if (type == "preorder") 
                                op_new = temp + op1 + op2;
                            else if (type == "postorder") 
                                op_new = op1 + op2 + temp;

                            st_Operand.Push(op_new);

                            st_Operator.Push(text[l]);
                        }
                        else
                        {
                            if (text[l] == '*' || text[l] == '/')
                            {
                                st_Operator.Push(text[l]);
                            }
                            else
                            {
                                temp = st_Operator.Pop(); //取出運算子
                                op2 = st_Operand.Pop();
                                op1 = st_Operand.Pop();

                                if (type == "preorder") 
                                    op_new = temp + op1 + op2;
                                else if (type == "postorder") 
                                    op_new = op1 + op2 + temp;

                                st_Operand.Push(op_new);

                                st_Operator.Push(text[l]);
                            }
                        }
                    }
                }

                l++;
            }

            st_Operand.Push(_operand); //將最後一組數字加入stack中

            //將剩下的運算元和運算子合併
            while (st_Operator.Any())
            {
                temp = st_Operator.Pop();
                op2 = st_Operand.Pop();
                op1 = st_Operand.Pop();

                if (type == "preorder")
                    op_new = temp + op1 + op2;
                else if (type == "postorder")
                    op_new = op1 + op2 + temp;

                st_Operand.Push(op_new);
            }

            if (type == "preorder")
                TextBox_preorder.Text = st_Operand.Pop();
            else if (type == "postorder")
                TextBox_postorder.Text = st_Operand.Pop();
        }

        private void Decimal(string text)
        {
            int l = 0;
            string _operand = ""; //找連續的數字
            string op1 = "", op2 = ""; //operand1 and operand2
            char temp = ' '; // temporary operator
            string op_new = ""; //merge operands and temporary operator
            int ans = 0;

            //掃瞄一輪
            while (l < text.Length)
            {
                if (!ifOperator(text[l])) //if not an operator
                {
                    _operand = _operand + text[l]; //如果下一個字元也是數字則合併一起
                }
                else
                {
                    st_Operand.Push(_operand);
                    _operand = "";

                    if (!st_Operator.Any()) //if stack is empty
                    {
                        st_Operator.Push(text[l]);
                    }
                    else
                    {
                        if (st_Operator.Peek() == '*' || st_Operator.Peek() == '/')
                        {
                            temp = st_Operator.Pop(); //取出運算子
                            op2 = st_Operand.Pop();
                            op1 = st_Operand.Pop();
                            ans = Computation(op1, op2, temp);
                            st_Operand.Push(ans.ToString());
                            st_Operator.Push(text[l]);
                        }
                        else
                        {
                            if (text[l] == '*' || text[l] == '/')
                            {
                                st_Operator.Push(text[l]);
                            }
                            else
                            {
                                temp = st_Operator.Pop(); //取出運算子
                                op2 = st_Operand.Pop();
                                op1 = st_Operand.Pop();
                                ans = Computation(op1, op2, temp);
                                st_Operand.Push(ans.ToString());
                                st_Operator.Push(text[l]);
                            }
                        }
                    }
                }

                l++;
            }

            st_Operand.Push(_operand); //將最後一組數字加入stack中

            //將剩下的運算元和運算子合併
            while (st_Operator.Any())
            {
                temp = st_Operator.Pop();
                op2 = st_Operand.Pop();
                op1 = st_Operand.Pop();
                ans = Computation(op1, op2, temp);
                st_Operand.Push(ans.ToString());
            }

            TextBox_decimal.Text = st_Operand.Pop();

            Binary(Convert.ToInt32(TextBox_decimal.Text));
        }

        private void Binary(int dec)
        {
            TextBox_binary.Text = Convert.ToString(dec, 2);
        }

        #region Buttons Setting

        private void Button_num_1_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "1";
        }

        private void Button_num_2_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "2";
        }

        private void Button_num_3_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "3";
        }

        private void Button_num_4_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "4";
        }

        private void Button_num_5_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "5";
        }

        private void Button_num_6_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "6";
        }

        private void Button_num_7_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "7";
        }

        private void Button_num_8_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "8";
        }

        private void Button_num_9_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "9";
        }

        private void Button_num_0_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "0";
        }

        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "+";
        }

        private void Button_sub_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "-";
        }

        private void Button_mul_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "*";
        }

        private void Button_div_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text + "/";
        }

        private void Button_del_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = TextBox_answer.Text.Remove(TextBox_answer.Text.Length - 1);
        }

        private void Button_ac_Click(object sender, RoutedEventArgs e)
        {
            TextBox_answer.Text = "";
        }

        #endregion Buttons Setting

        #region Function Buttons Setting

        private void Button_enter_Click(object sender, RoutedEventArgs e)
        {
            string text = TextBox_answer.Text;
            int text_length = text.Length;


            Traversal("preorder", text);
            Traversal("postorder", text);
            Decimal(text);
        }

        private void Button_query_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            this.Hide();
            window1.Show();
        }

        private void Button_insert_Click(object sender, RoutedEventArgs e)
        {
            string connString = "datasource=127.0.0.1;port=3306;username=root;password=;database=c#";

            MySqlConnection conn = new MySqlConnection(connString);

            try
            {
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT * FROM calculator WHERE expression = @expression";
                cmd.Parameters.AddWithValue("@expression", TextBox_answer.Text);
                MySqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    MessageBox.Show("The expression already exists!!");
                    sdr.Close();
                    conn.Close();
                }
                else
                {
                    sdr.Close();
                    cmd.CommandText = "SELECT id FROM calculator ORDER BY id DESC LIMIT 1;";
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    id++;
                    cmd.CommandText = "INSERT INTO calculator(id, expression, preorder, postorder, ans_decimal, ans_binary) VALUES " +
                            "(@id, @expression, @preorder, @postorder, @decimal, @binary)";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@preorder", TextBox_preorder.Text);
                    cmd.Parameters.AddWithValue("@postorder", TextBox_postorder.Text);
                    cmd.Parameters.AddWithValue("@decimal", TextBox_decimal.Text);
                    cmd.Parameters.AddWithValue("@binary", TextBox_binary.Text);

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    MessageBox.Show("Successfully insert!!");
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        #endregion Function Buttons Setting


    }
}
