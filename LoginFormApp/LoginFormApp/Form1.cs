using Microsoft.Data.SqlClient;

namespace LoginFormApp;

public sealed class Form1 : Form
{
    private readonly TextBox txtUsername = new();
    private readonly TextBox txtPassword = new();
    private readonly CheckBox chkShowPassword = new();
    private readonly Button btnLogin = new();
    private readonly Button btnRegister = new();
    private readonly Button btnExit = new();
    private readonly ErrorProvider errorProvider = new();

    public Form1()
    {
        Text = "Đăng nhập hệ thống";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(520, 320);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        AcceptButton = btnLogin;
        CancelButton = btnExit;

        var heading = new Label
        {
            Text = "ĐĂNG NHẬP HỆ THỐNG",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(125, 28)
        };

        var lblUsername = new Label
        {
            Text = "Tên đăng nhập:",
            AutoSize = true,
            Location = new Point(65, 90)
        };

        txtUsername.Name = "txtUsername";
        txtUsername.Location = new Point(180, 86);
        txtUsername.Size = new Size(260, 27);

        var lblPassword = new Label
        {
            Text = "Mật khẩu:",
            AutoSize = true,
            Location = new Point(65, 132)
        };

        txtPassword.Name = "txtPassword";
        txtPassword.Location = new Point(180, 128);
        txtPassword.Size = new Size(260, 27);
        txtPassword.UseSystemPasswordChar = true;

        chkShowPassword.Name = "chkShowPassword";
        chkShowPassword.Text = "Hiện mật khẩu";
        chkShowPassword.AutoSize = true;
        chkShowPassword.Location = new Point(180, 163);
        chkShowPassword.CheckedChanged += (_, _) =>
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

        btnLogin.Name = "btnLogin";
        btnLogin.Text = "Đăng nhập";
        btnLogin.Size = new Size(105, 36);
        btnLogin.Location = new Point(65, 220);
        btnLogin.Click += BtnLogin_Click;

        btnRegister.Name = "btnRegister";
        btnRegister.Text = "Đăng ký";
        btnRegister.Size = new Size(105, 36);
        btnRegister.Location = new Point(205, 220);
        btnRegister.Click += BtnRegister_Click;

        btnExit.Name = "btnExit";
        btnExit.Text = "Thoát";
        btnExit.Size = new Size(105, 36);
        btnExit.Location = new Point(345, 220);
        btnExit.Click += (_, _) => Close();

        errorProvider.ContainerControl = this;

        Controls.AddRange([
            heading, lblUsername, txtUsername, lblPassword, txtPassword,
            chkShowPassword, btnLogin, btnRegister, btnExit
        ]);
    }

    private bool ValidateInputs()
    {
        errorProvider.Clear();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            errorProvider.SetError(txtUsername, "Vui lòng nhập tên đăng nhập.");
            valid = false;
        }
        else if (txtUsername.Text.Trim().Length > 50)
        {
            errorProvider.SetError(txtUsername, "Tên đăng nhập tối đa 50 ký tự.");
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            errorProvider.SetError(txtPassword, "Vui lòng nhập mật khẩu.");
            valid = false;
        }

        return valid;
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        if (!ValidateInputs()) return;

        try
        {
            bool success = AuthService.Login(txtUsername.Text.Trim(), txtPassword.Text);
            if (success)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.",
                    "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
        catch (SqlException ex) when (ex.Number is 2601 or 2627)
        {
            MessageBox.Show("Tên đăng nhập đã tồn tại.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception)
        {
            MessageBox.Show(
                "Không thể kết nối cơ sở dữ liệu. Hãy kiểm tra SQL Server LocalDB và chạy file database.sql.",
                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnRegister_Click(object? sender, EventArgs e)
    {
        if (!ValidateInputs()) return;

        if (txtPassword.Text.Length < 8)
        {
            errorProvider.SetError(txtPassword, "Mật khẩu phải có ít nhất 8 ký tự.");
            return;
        }

        try
        {
            AuthService.Register(txtUsername.Text.Trim(), txtPassword.Text);
            MessageBox.Show("Đăng ký tài khoản thành công. Bạn có thể đăng nhập.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtPassword.Clear();
            txtPassword.Focus();
        }
        catch (SqlException ex) when (ex.Number is 2601 or 2627)
        {
            errorProvider.SetError(txtUsername, "Tên đăng nhập đã tồn tại.");
        }
        catch (Exception)
        {
            MessageBox.Show(
                "Không thể lưu tài khoản. Hãy kiểm tra SQL Server LocalDB và chạy file database.sql.",
                "Lỗi cơ sở dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
