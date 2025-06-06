namespace WindowsFormsApp22
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableSelector = new System.Windows.Forms.ComboBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();

            // tableSelector
            this.tableSelector.Location = new System.Drawing.Point(10, 10);
            this.tableSelector.Size = new System.Drawing.Size(200, 21);

            // dataGridView
            this.dataGridView.Location = new System.Drawing.Point(10, 40);
            this.dataGridView.Size = new System.Drawing.Size(600, 300);
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(10, 350);
            this.btnAdd.Size = new System.Drawing.Size(100, 23);
            this.btnAdd.Text = "Добавить";

            // btnUpdate
            this.btnUpdate.Location = new System.Drawing.Point(120, 350);
            this.btnUpdate.Size = new System.Drawing.Size(100, 23);
            this.btnUpdate.Text = "Обновить";

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(230, 350);
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.Text = "Удалить";

            // Form1
            this.ClientSize = new System.Drawing.Size(620, 400);
            this.Controls.Add(this.tableSelector);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ComboBox tableSelector;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
    }
}

