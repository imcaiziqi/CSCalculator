using System;

using System.Drawing;

using System.Windows.Forms;

namespace Calc
{
    public partial  class Form1 : Form
    {
      
        public Form1()
        {
            InitializeComponent();
          
        }

        // 重写 WndProc 方法  此方法只为不能随意拖动大小用，因为没有适应大小的样式
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112; // 系统命令
            const int SC_SIZE = 0xF000;       // 调整大小命令

            if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt32() & 0xFFF0) == SC_SIZE)
            {
                // 禁止调整大小
                m.Result = IntPtr.Zero;
                return;
            }

            base.WndProc(ref m);
        }


        //变量声明开始

        //记录程序第一次1启动
        Boolean system_one = false;


        //判断用过小数点后,是否有输入数字如果没有输入数字就不能使用四则运算符
        Boolean dian_number = false;

        //newValue 用来记录每一次的新值
        decimal newValue = 0.0m;

        //value 是最后的结果
        decimal value=0.0m;


        //fh 是用来记录我们正在使用的符号
        String fh;

        //eco 用来记录 用户摁过多少次等于号, eco = 0 就是还没用过, 如果用过的话  就++
        int eco = 0;

        //oftenValue 用来记录  一直被处理的数字
        decimal oftenValue = 0.0m;

        //record 用来判断使用四则运算符后,是否输入数字,输入了就是true没有就是false
        Boolean record = true;

        //numberButton 是用来判断数字键是否使用过,numberButton = false 就是没用过 等于 numberButton =true 就是使用过;
        Boolean numberButton = false;

        //符号的次数  false就按原来的样式  true就是其他样式
        Boolean numberOfSymbols = false;

        //用来装记忆的盒子
        decimal memory = 0.0m;
        //等号执行多次的 num1 
        decimal num1 = 0.0m;

        //记录符号中的变化之前的值
        decimal temp_symbols;


        String tempDingyu = "";


        //--------------------------------------  变量声明结束 -----------------------------------------------





        //等于号
        /*
         * 处理点了等号之后的操作
         * 比如如果我们没有输入值,然后点了等号  之后的处理
         */
        private void Equal_Click(object sender, EventArgs e)
        {



            //读取等于号
            Button nu = (Button)sender;

            //在使用等号之前没有使用 数字键
            if (numberButton == false)
            {   
                //Enter_textBox1 就等于原来的值
                Enter_textBox1.Text = value.ToString();
            }


            /*  此处也做过处理  不在会等于空值所以选择弃用    
             *  
             *  
             * 
             * 
             * 
             //如果Enter_textBox1.Text  是空值的话  Enter_textBox1.Text 等于 "0"
            if (Enter_textBox1.Text == "")
            {
                Enter_textBox1.Text = "0";
            }
            else//不是的话就,正常执行操作
            {

            }

            */

            //如果符号没用过  并且  系统第一次启动
            if (numberOfSymbols == false & system_one == false)
            {
                //那么值Enter_textBox1.Text 的值是多少   value就等于多少
                value = Convert.ToDecimal(Enter_textBox1.Text);
                //然后再显示到Out_label1上
                Out_label1.Text = Enter_textBox1.Text;

            }
            else
            {   
                //第一次点等号执行这个操作
                if (eco == 0)
                {


                    //判断Enter_textBox1 上的数字是否是数字
                    Boolean WhetherTheNumbers = decimal.TryParse(Enter_textBox1.Text, out decimal Now);

                    //是数字就执行
                    if (WhetherTheNumbers == true)
                    {   //当fh里面没有指向哪个具体的符号就执行
                        if (fh == null)
                        {
                            //Out_label1的值等于 Enter_textBox1上的值
                            Out_label1.Text = Enter_textBox1.Text;
                            //并且 让numberOfSymbols  等于false 就是 没有用过符号
                            numberOfSymbols = false;
                            //Enter_textBox1的值等于 value
                            Enter_textBox1.Text = value.ToString();
                            //然后退出
                            return;
                        }

                        //显示样式 类似这样 (2+3=)
                        Out_label1.Text = value.ToString() + fh.ToString() + Enter_textBox1.Text + nu.Text;

                        //判断之前输入的符号是哪个  然后执行哪个
                        switch (fh)
                        {
                            case "+":
                                value += Now;

                                break;
                            case "-":
                                value -= Now;
                                break;
                            case "x":
                                value *= Now;

                                break;
                            case "÷":

                                if (Enter_textBox1.Text == "0")
                                {
                                    Enter_textBox1.Text = "除数不能为零";
                                    return;
                                }
                                else
                                {
                                    value /= Now;
                                }
                                break;

                        }

                        eco = 1;

                        //保留这次被处理的值
                        oftenValue = Now;
                    }
                    else//如果不是数字
                    {   
                        //就将全部都赋值为 0
                        Enter_textBox1.Text = "0";
                        Out_label1.Text = "0";
                        oftenValue = 0;
                        //然后再退出
                        return;
                    }
                }//点击1~n次的等号后的操作
                else
                {
                    Decimal oldValue = Convert.ToDecimal(Enter_textBox1.Text);
                    switch (fh)
                    {
                        case "+":
                            
                            value = oldValue + oftenValue;
                            num1 = oldValue;
                            break;
                        case "-":
                            
                            value = oldValue - oftenValue;
                            num1 = oldValue;
                            break;
                        case "x":
                            
                            value = oldValue * oftenValue;
                            num1 = oldValue;
                            break;
                        case "÷":

                            if (Enter_textBox1.Text == "0")
                            {
                                Enter_textBox1.Text = "除数不能为零";
                            }
                            else
                            {
                                value = oldValue / oftenValue;
                            }
                            num1 = oldValue;

                            // 转换为字符串来检查小数部分的长度
                            string resultStr = value.ToString("G"); // "G" 格式可以去掉不必要的尾随零
                            int decimalIndex = resultStr.IndexOf('.');

                            if (decimalIndex >= 0) // 如果是小数
                            {
                                string decimalPart = resultStr.Substring(decimalIndex + 1);
                                if (decimalPart.Length > 7)
                                {
                                    // 如果小数部分超过7位，就四舍五入保留7位
                                    value = Math.Round(value, 8);
                                }
                            }

                            break;

                    }
                    //MessageBox.Show(num1.ToString());
                    Out_label1.Text = num1.ToString() + fh.ToString() + oftenValue.ToString() + nu.Text;
                }

            }

            Enter_textBox1.Text = value.ToString();

            

            numberOfSymbols = false;
            //使用过一次就赋值 1
            eco++;
            system_one = true;

            tempDingyu = "="; 

        }
        //___________________________ 等号 _________________________________________________


        //符号键入方法
        private void Symbol_Click(object sender, EventArgs e)
        {
            //当用过小数点后,使用了数字键,才可以使用符号
            if (dian_number == true)
            {
                //刷新用户摁过的等号次数

                //向下转型,获得到该符号
                Button f = (Button)sender;
                fh = f.Text;

                /*这里因为我处理过了,所以不可能会出现空值得情况所以就可以弃用了
                 * 如果Enter_textBox1.Text  是空值的话  让Enter_textBox1.Text 等于 原来的值
                 *//*
                if (Enter_textBox1.Text == "")
                {
                    Enter_textBox1.Text = newValue.ToString();
                }*/


                //第一次使用从这里
                if (numberOfSymbols == false)
                {
                    Out_label1.Text = "";
                    //record 记录数字键是否有用过   有用过就是true  没用过就是false
                    if (record == true)
                    {
                        newValue = decimal.Parse(Enter_textBox1.Text);
                        //Out_label1 的内容是不是0 如果是就直接覆盖  如果不是就加等
                        if (Out_label1.Text == "0" && eco != 0)
                        {

                            Out_label1.Text = Enter_textBox1.Text + fh;
                        }
                        else
                        {
                            Out_label1.Text += Enter_textBox1.Text + fh;
                        }

                        //获取到  第一次执行过获得得值
                        value = newValue;

                        //用完符号键 就需要在等待数字键的输入所以此时数字键是没用过的所以是false
                        record = false;
                    }
                    else
                    {   //没有用过数字键 就让Enter_textBox1 等于 0
                        Enter_textBox1.Text = "0";
                    }

                }
                else
                {

                    if (record == true)
                    {
                        newValue = decimal.Parse(Enter_textBox1.Text);
                        temp_symbols = value;
                        switch (fh)
                        {
                            case "+":
                                value += newValue;
                                break;
                            case "-":
                                value -= newValue;

                                break;
                            case "x":
                                value *= newValue;

                                break;
                            case "÷":
                                if (Enter_textBox1.Text == "0")
                                {
                                    Enter_textBox1.Text = "除数不能为零";
                                }
                                else
                                {
                                    value /= newValue;
                                }

                                break;

                        }
                        Enter_textBox1.Text = value.ToString();
                        /*CalculationSteps.Text = enter.Text + f.Text;*/
                        //该之后
                        Out_label1.Text = temp_symbols.ToString() + f.Text + newValue.ToString() + "=";


                    }
                    else
                    {
                        Out_label1.Text = Enter_textBox1.Text + f.Text;
                        record = false;
                    }
                }


                numberButton = false;//符号键入后需要继续输入数字所以就把数字的使用记录清空后,在等待数字键入,以防出现两次符号
            }
            else
            {
                //如果输入小数点后不输入数字,就不做任何操作 
            }


            //用一次
            numberOfSymbols = true;
          
        }
        //___________________________________ 符号键入方法 _____________________________________________



        //清空操作
        private void Empty_Click(object sender, EventArgs e)
        {
            Button en = (Button)sender;

            switch (en.Text)
            {   //让一切回到最初的样式
                case "C":

                    newValue = 0.0m;
                    eco = 0;
                    oftenValue = 0.0m;
                    numberButton = false;
                    value = 0.0m;
                    record = true;
                    Enter_textBox1.Text = "0";
                    dian_number = true;
                    Out_label1.Text = "";
                    break;
                // 只清空enter.Text的记录
                case "CE":
                    Enter_textBox1.Text = "0";
                    break;

            }

        }
        //___________________________ 清空操作方法结束 _________________________________________________

        //小数点
        private void Dian_Click(object sender, EventArgs e)
        {

            //判断该字符串中是否有 "." 如果没有就可以使用 "."  如果有就不能使用
            if (Enter_textBox1.Text.Contains(".") == false)
            {   
                Enter_textBox1.Text += ".";

            }
                //判断是否使用过等于号
            if (eco == 1)
            {   //如果使用过等于号,在判断Enter_textBox1 上是否还有 小数点
                if (Enter_textBox1.Text.Contains(".") == true)
                {   
                    //如果有,就清空 Out_label1
                    Out_label1.Text = "";
                    //给Enter_textBox1 上的text 赋值为 0.
                    value = 0;
                    num1 = 0;
                    newValue = 0;
                    temp_symbols = 0;
                    Enter_textBox1.Text = "0.";
                }
            }
            //使用小数点后,但是还没有用过数字键所以 false 
            dian_number = false;

        }
        //___________________________ 小数点方法结束 _________________________________________________




        //数字键入
        private void Number_Click(object sender, EventArgs e)
        {

            if (tempDingyu.Equals("=")) {
                decimal s = value;
                Empty_Click(C, new EventArgs());
                value = s;
                tempDingyu = "";
            }

            dian_number = true;

            //在用过四则运算符后,使用一次数字 record 就等于 true
            record = true;
            //获取一下Enter_textBox1  已经输入的数字长度
            int leng = Enter_textBox1.TextLength;
            //一样向下转型
            Button num = (Button)sender;
            //如果没用过数字键,那就先清空一下原来Enter_textBox1上面的内容
            if (numberButton == false)
            {

                Enter_textBox1.Clear();

            }

            //判断长度是否小于16
            if (leng < 16)
                {   //然后再判断Enter_textBox1 上的数字是否是0
                    if (Enter_textBox1.Text == "0")
                    {   //是的话就用我们键入的数字键替换掉 0
                        Enter_textBox1.Text = num.Text;
                    }
                    else
                    {   //不是 0 的话就加在后面
                        Enter_textBox1.Text += num.Text;
                    }
            }
            else
            {   //如果大于16了 那么我们就让它等于它原来的数,不在增加了
                Enter_textBox1.Text =  Enter_textBox1.Text;
            }

            numberButton = true;//用过就记录一下
        }
        //___________________________ 数字键方法结束 _________________________________________________


        //退格键方法
        private void Back_Click(object sender, EventArgs e)
        {
                //当删完了之后,就给它补个零
                if (Enter_textBox1.Text.Length == 1)
                {
                    Enter_textBox1.Text = "0";
                }
                else  //否则就继续删
                { 
                
                
                //把当前的内容存进一个字符串里
                   string old = Enter_textBox1.Text;
                    char o = old[0];
                    //判断是否只剩两个字符,如果是的话 就判断他前面是否有符号,有的话就一起删掉
                    if (old.Length == 2 && o == '-')
                    {
                        Enter_textBox1.Text = old.Remove(0);
                        Enter_textBox1.Text = "0";
                    }
                    else  //没有的话就从最后一个字符位置开始删
                    {
                        Enter_textBox1.Text = old.Remove(old.Length - 1);
                    }

                }

          }
        //___________________________________ 退格键方法结束 ___________________________________



        //百分比方法
        private void Baifenbi_Click(object sender, EventArgs e)
        {

            int B = 100;

            Enter_textBox1.Text = (double.Parse(Enter_textBox1.Text) / B).ToString();
        }
        //___________________________________ 百分比方法结束 ___________________________________


        //动态显示
        /*
         该方法的作用是控制Enter_textBox1
        也就是用户能够输入几个数
        超过十个数之后就缩小字体大小
         
         */
        private void Enter_textBox1_TextChanged(object sender, EventArgs e)
        {   
            //向下转型,就可以使用textBox的属性了
            TextBox temp = (TextBox)sender;
            
            //查看我们输入了多少个数字
            int leng = temp.TextLength;

            //如果数字大于10个数就,让字体变小
            if (leng == 10)
            {
                //设置enter 超过10位时 字体变小
                Enter_textBox1.Font = new Font("Microsoft YaHei UI", 22);
            }
        }
        //___________________________________ 动态显示方法结束 __________________________________


        //正负号添加
        private void JiaOrJian_Click(object sender, EventArgs e)
        {
            //先把Enter_textBox1上的数值,存进tempStr 字符串中
            string tempStr = Enter_textBox1.Text;

            //然后用 一个字符 t 存入 tempStr 的第一个字符
            char t = tempStr[0];
            //如果Enter_textBox1 只有一个0,那么就让它继续等于0
            if (Enter_textBox1.Text == "0")
            {
                Enter_textBox1.Text = Enter_textBox1.Text;
            }
            else
            {   //判断如果是负号,我们就把它删掉,就变成正数了
                if (t == '-')
                {
                    Enter_textBox1.Text = tempStr.Remove(0, 1);

                }
                else
                {
                    //否者就给它一个负号,变成负数
                    Enter_textBox1.Text = tempStr.Insert(0, "-");

                }

            }
        }
        //___________________________________ 正负号添加方法结束 __________________________________


        //记忆键
        private void MemoryKeys(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            switch (label.Text) {
                case "MC":
                    memory = 0.00m;
                    MC_Label.Enabled = false;
                    Mv_label.Enabled = false;
                    MR_label.Enabled = false;
                    break;
                case "MR":
                    Enter_textBox1.Text = memory.ToString();
                    break;
                case "M+":
                    memory += decimal.Parse(Enter_textBox1.Text);
                    break;
                case "M-":
                    memory -= decimal.Parse(Enter_textBox1.Text);
                    break;
                case "MS":
                    MR_label.Enabled = true;
                    MC_Label.Enabled = true;
                    Mv_label.Enabled = true;
                    memory = decimal.Parse(Enter_textBox1.Text);
                   
                    break;
                case "Mv":
                    MessageBox.Show("当前记忆数字是" + memory.ToString(), "当前记忆数字");
                    break;
            }
           
        }


        //___________________________________ 记忆键方法结束 __________________________________


        private void Form1_Load(object sender, EventArgs e)
        {
            //设置这个窗体的不透明度
            this.Opacity = 1;
        }

    }





}
