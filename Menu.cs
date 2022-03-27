using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TimeGame
{
    public partial class Menu : Form
    {
        private TimeGame game;

        public Menu(TimeGame game)
        {
            InitializeComponent();
            this.game = game;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            game.Unpause();
        }
    }
}
