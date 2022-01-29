private void HandleButtonClick(object sender, EventArgs e)
{
    if (this.passportTextbox.Text.Trim() == "")
    {
        int num1 = GetNumberFromMessageBox("Введите серию и номер паспорта");
    }
    else
    {
        string rawData = this.passportTextbox.Text.Trim().Replace(" ", string.Empty);
        
        if (rawData.Length < 10)
        {
            ShowResult("Неверный формат серии или номера паспорта");
        }
        else
        {
            ShowUserStatus(rawData);
        }
    }
}

private void ShowUserStatus(string rawData)
{
    string commandText = string.Format("select * from passports where num='{0}' limit 1;", (object)Form1.ComputeSha256Hash(rawData));
    string connectionString = string.Format("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite");
    try
    {
        DataTable dataTable1 = GetDataTable(commandText, connectionString);

        if (dataTable1.Rows.Count > 0)
        {
            if (CheckStatus(dataTable1) == true)
                ShowResult("По паспорту «" + this.passportTextbox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН");
            else
                ShowResult("По паспорту «" + this.passportTextbox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ");
        }
        else
        {
            ShowResult("Паспорт «" + this.passportTextbox.Text + "» в списке участников дистанционного голосования НЕ НАЙДЕН");
        }
    }
    catch (SQLiteException ex)
    {
        if (ex.ErrorCode != 1)
            return;
        int num2 = GetNumberFromMessageBox("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");
    }
}

private DataTable GetDataTable(string commandText, string connectionString)
{
    SQLiteConnection connection = new SQLiteConnection(connectionString);
    connection.Open();
    SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));
    DataTable dataTable1 = new DataTable();
    DataTable dataTable2 = dataTable1;
    sqLiteDataAdapter.Fill(dataTable2);
    connection.Close();
    return dataTable1;
}

private bool CheckStatus(DataTable dataTable)
{
    return Convert.ToBoolean(dataTable1.Rows[0].ItemArray[1]);
}

private void ShowResult(string result)
{
    this.textResult.Text = "Неверный формат серии или номера паспорта";
}

private int GetNumberFromMessageBox(string message)
{
    return (int)MessageBox.Show(message);
}