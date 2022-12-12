using System.Drawing.Drawing2D;
using System.Media;
using System.Drawing;



namespace YOUR_NAMESPACE
{
    public partial class ChatBox : UserControl
    {
      


        public class Message
        {
            public string Text { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }

            public bool me { get; set; }
            public bool Seen { get; set; }

            public Message(bool me, string text, string name, DateTime date, bool seen)
            {
                this.Text = text;
                this.Name = name;
                this.me = me;
                this.Date = date;
                this.Seen = seen;
            }
        }
        Panel spacer = new Panel();
        TextBox textInput = new TextBox();
        Button sendBtn = new Button();
        private FlowLayoutPanel panel1 = new FlowLayoutPanel();

        private List<Message> messages = new List<Message>();
    
        public delegate void ChatSendEventHandler(object sender, ChatSendArgs e);
        public event ChatSendEventHandler ChatSend;
        public class ChatSendArgs : EventArgs
        {
            public string Message { get; set; }
        }
        protected virtual void OnChatSend(string message)
        {
            var e = new ChatSendArgs();
            e.Message = message;
          


            ChatSend?.Invoke(this, e);
        }
        public ChatBox()
        {


            InitializeComponent();
            this.Margin = new Padding(0);
            this.Padding = new Padding(0);
            this.Margin = new Padding(0, 0, 0, 0);

            panel1.AutoScroll = true;

       
        }
       
        public void AddMessage(bool me, string text, string name, DateTime date, bool seen)
        {
             
            messages.Add(new Message(me, text, name, date, seen));
            UpdateUserControl(new Message(me, text, name, date, seen));
    
        }


       
        private async void UpdateUserControl(Message message)
        {/*


            Regex regex = new Regex(@"\[(img|file)\s+url=([^\]]+)\]");

            // Use the regex to find a match in the input string
            Match match = regex.Match(input);

            // If a match was found, extract the URL from the match
            if (match.Success)
            {
                string url = match.Groups[2].Value;
                Console.WriteLine(url); // Outputs "http://example.com"
            }*/

            panel1.SuspendLayout();
            
        
                
                FlowLayoutPanel messagePanel = new FlowLayoutPanel();
                messagePanel.FlowDirection = FlowDirection.TopDown;
                // Align the panel to the left or right side of the user control
                messagePanel.AutoSize = true;
                messagePanel.Padding = new Padding(10);
                // Create a label for the message text
                Label textLabel = new Label();
                Label senderLabel = new Label();
           

            if (message.me)
                {

                    messagePanel.Dock = DockStyle.Right;
                    messagePanel.Margin = new Padding(200, 13, 0, 13);
                  
                } else
                {
                messagePanel.Margin = new Padding(0, 13, 200, 13);
                messagePanel.Dock = DockStyle.Left;
              
                }
            

                messagePanel.Paint += (sender, e) =>
                {
                    GraphicsPath path = new GraphicsPath();

                    if (message.me)
                    {
                        //thats top right when is "me"
                        path.AddArc(0, 0, 50, 50, 180, 90);
                        path.AddLine(messagePanel.Width - (50 + 1), 0, messagePanel.Width, 0);
                        path.AddLine(messagePanel.Width, 50, messagePanel.Width, messagePanel.Height - (50 + 1));
                        path.AddArc(messagePanel.Width - (80 + 1), messagePanel.Height - (80 + 1), 80, 80, 0, 90);
                        path.AddLine(messagePanel.Width - (50 + 1), messagePanel.Height, 50, messagePanel.Height);
                        path.AddArc(0, messagePanel.Height - (50 + 1), 50, 50, 90, 90);

                    }
                    else
                    {
                        path.AddArc(0, 0, 1, 1, 180, 90);
                        path.AddLine(50, 0, messagePanel.Width - (50 + 1), 0);
                        path.AddArc(messagePanel.Width - (50 + 1), 0, 50, 50, 270, 90);
                        path.AddLine(messagePanel.Width, 50, messagePanel.Width, messagePanel.Height - (50 + 1));
                        path.AddArc(messagePanel.Width - (50 + 1), messagePanel.Height - (50 + 1), 50, 50, 0, 90);
                        path.AddLine(messagePanel.Width - (50 + 1), messagePanel.Height, 50, messagePanel.Height);
                        path.AddArc(0, messagePanel.Height - (80 + 1), 80, 80, 90, 90);
                    }
                    path.CloseFigure();
                    messagePanel.Region = new Region(path);
                };

            //Out of paint
            textLabel.Text = message.Text;
            textLabel.BackColor = Color.Transparent;
           
            textLabel.AutoSize = true;

            if (message.me)
            {
                messagePanel.BackColor = ColorTranslator.FromHtml("#3474eb");
                textLabel.Dock = DockStyle.Left;
                senderLabel.Dock = DockStyle.Right;
                textLabel.ForeColor = Color.White;
                senderLabel.ForeColor = Color.White;
                senderLabel.Text = $"{message.Date:MM/dd/yyyy HH:mm}";
                if (message.Seen) senderLabel.Text += " ✔️";


            } else
            {
                play_ting();
                senderLabel.Dock = DockStyle.Bottom;
                textLabel.Dock = DockStyle.Right;
                messagePanel.BackColor = ColorTranslator.FromHtml("#e4e8f0");
                senderLabel.Text = $"~ {message.Name} {message.Date:MM/dd/yyyy HH:mm}";

            }
        
            messagePanel.Controls.Add(textLabel);
          

            senderLabel.Font = new Font(textLabel.Font.FontFamily, 7);
                senderLabel.BackColor = Color.Transparent;
              
                messagePanel.Controls.Add(senderLabel);
                panel1.Controls.Add(messagePanel);

          /*  
		trying to support emojis, a directory /emojis 
                   cointains the unicode of the emoji .png
                    eg. "🙂.png"  
    

          textLabel.Paint += (object sender, PaintEventArgs e) =>
            {
             

                string[] filenames = Directory.GetFiles("./emojis");
              
                foreach (string emoji in filenames)
                {
                    string emojit = emoji;
                    emojit = emojit.Replace("./emojis/", "");
                    emojit = emojit.Replace(@"./emojis\", "");
                    emojit = emojit.Replace(@".png", "");
                    // Find the location of the emoji within the text.

                    var emojiIndex = textLabel.Text.IndexOf(emojit);
                    if (emojiIndex == -1)
                    {
                        // The emoji was not found in the text, so there is nothing to do.
                        continue;
                    }
                    if (emojiIndex == 0)
                    {
                        emojiIndex = 1;
                        textLabel.Text = " " + textLabel.Text;
                    }
                  
                    var picture = Image.FromFile(emoji);

                    var textSizeThum = e.Graphics.MeasureString(emojit, textLabel.Font);
                    var thumbnail = picture.GetThumbnailImage((int)textSizeThum.Height + 2, (int)textSizeThum.Height + 2, null, IntPtr.Zero);
                        var textBeforeEmoji = textLabel.Text.Substring(0, emojiIndex);


                        var textBeforeEmojiSize = e.Graphics.MeasureString(textBeforeEmoji, textLabel.Font);


                        // Calculate the location of the emoji within the text.
                        var emojiX = (int)textBeforeEmojiSize.Width;
                        var emojiY = (textLabel.Height - (int)textBeforeEmojiSize.Height) / 2;
                        // Draw the picture on the control at the location of the emoji within the text.
                         e.Graphics.DrawImage(thumbnail, new Point(emojiX, emojiY));
                }

                
            };*/

                     panel1.ResumeLayout(true);
        

            panel1.VerticalScroll.Value = panel1.VerticalScroll.Maximum;


          
        }

   
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void play_ting()
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream s = a.GetManifestResourceStream("foodsoda.ting.wav");
            SoundPlayer player = new SoundPlayer(s);
            player.Play();
        }
        private void ChatBox_Load(object sender, EventArgs e)
        {
          


            panel1.ClientSizeChanged += (sender, e) =>
            {
                spacer.Width = panel1.ClientSize.Width;
            };

            panel1.Padding = new Padding(0);
            panel1.Margin = new Padding(0);

            sendBtn.Dock = DockStyle.Right;
            sendBtn.AutoSize = true;
            sendBtn.Text = "Send";
            sendBtn.Margin = new Padding(0);
            sendBtn.Padding = new Padding(0);
            sendBtn.FlatStyle = FlatStyle.Flat;

            sendBtn.AutoSize = true;

            textInput.Font = new Font(textInput.Font.FontFamily, 13);
            sendBtn.Height = textInput.Height;
            textInput.Dock = DockStyle.Left;
            textInput.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        
            textInput.Width = this.Width - sendBtn.Width;
              


            textInput.Visible = true;
          panel1.Width = this.Width;
          panel1.Height = this.Height - textInput.Height;
            sendBtn.Click += SendBtn_Click;
            this.Resize += (object sender, EventArgs e) =>
            {
                panel1.Width = this.Width;
                panel1.Height = this.Height;
                panel1.Height = this.Height - textInput.Height;
                textInput.Width = this.Width - sendBtn.Width;
            };
            this.DoubleBuffered = true;

            this.AutoScroll = false;
     
            panel1.FlowDirection = FlowDirection.TopDown;
        
            panel1.AutoSize = false;

            panel1.Width = this.Width;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;


       
            panel1.AutoScroll = true;
            panel1.Dock = DockStyle.Top;
            panel1.Anchor = AnchorStyles.Left | AnchorStyles.Top;


            panel1.WrapContents = false;
         
          //  panel1.BackColor = Color.Red;
            
            spacer.Padding = new Padding(0);
            spacer.Margin = new Padding(0);
            spacer.Left = 0;
            spacer.Width = panel1.ClientSize.Width;
            spacer.Height = 2;
             panel1.Controls.Add(spacer);
            this.Controls.Add(sendBtn);
         

            this.Controls.Add(textInput);
            this.Controls.Add(panel1);

            textInput.KeyPress += (object sender, KeyPressEventArgs e) =>
            {
              
                if (e.KeyChar == (char)13)
                {
                    e.Handled = true;
                    sendBtn.PerformClick();

                    return;
                    // Enter key pressed
                }
            };

        }

        private void SendBtn_Click(object? sender, EventArgs e)
        {
          

            OnChatSend(textInput.Text);
            textInput.Text = "";
        }

       
    }
}
