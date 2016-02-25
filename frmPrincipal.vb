Imports System.Net.Mail
Imports System.IO
Imports Spire.Doc
Imports System.Text


Public Class frmPrincipal

    Dim Usuario As String
    Dim Pass As String
    Dim Port As Integer
    Dim Server As String
    Dim SSL As Boolean
    Dim Credenciales As Boolean
    Dim sRutaCV As String
    Dim sRutaCarta As String
    Dim sAsunto As String

    Private Const FICHEROINICIO As String = "Config.ini"

    Private Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles mnuEnviarMail.Click
        Dim i As Integer
        ToolStripProgressBar1.Maximum = ListView1.Items.Count
        For i = 0 To ListView1.Items.Count - 1

            EnvioMails(ListView1.Items(i).Text, i, ListView1.Items.Count)
            Application.DoEvents()
        Next
    End Sub

    Private Sub EnvioMails(sMail As String, progreso As Integer, total As Integer)

        lblEstado.Text = "Enviando correo " & (progreso + 1) & "/" & total
        Application.DoEvents()

        Dim oMsg As New MailMessage()

        oMsg.From = _
                New MailAddress("raulnavascues@yahoo.es")

        oMsg.To.Add(New MailAddress(sMail))

        oMsg.Priority = MailPriority.Normal

        oMsg.Subject = sAsunto '"Envío Curriculum Raúl Navascues Vega"

        oMsg.Body = TextBox1.Text 'sCartaPresentacion.ToString
        If txtCV.Text <> "" Then oMsg.Attachments.Add(New Attachment(txtCV.Text))
        oMsg.IsBodyHtml = False


        Dim client As New SmtpClient()
        client.Host = Server
        client.Port = Port
        client.EnableSsl = SSL
        client.UseDefaultCredentials = Credenciales
        client.Credentials = New System.Net.NetworkCredential(Usuario, Pass)

        Try
            client.Send(oMsg)
            Logger.i("Correo enviado correctamente a la dirección: " + sMail)
        Catch excepcion As System.Net.Mail.SmtpException
            Logger.e("Error de envío:\n", excepcion)
        Finally
            oMsg.Dispose()
            ToolStripProgressBar1.Value = progreso + 1
            lblEstado.Text = "Correo " & (progreso + 1) & "/" & total & " enviado (" + ((progreso + 1) * 100) / total + ")"
        End Try


    End Sub
    ''' <summary>
    ''' Abrir y cargar en el listview las direcciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDirecciones_Click(sender As Object, e As EventArgs) Handles btnDirecciones.Click
        Dim oAbrir As New OpenFileDialog

        oAbrir.AddExtension = True
        oAbrir.Filter = "Archivos de texto|*.txt|Todos los archivos|*.*"
        oAbrir.ShowDialog()

        txtDirecciones.Text = oAbrir.FileName

        ' Introducir las direcciones para hacer mailing en el LV
        Dim sPath = txtDirecciones.Text
        Dim sr As StreamReader = File.OpenText(sPath)
        Dim sLineaDirecciones As String

        sLineaDirecciones = sr.ReadToEnd()
        'ReadLine()

        sr.Close()
        Dim sDirecciones() As String = sLineaDirecciones.Split("||")
        ListView1.Items.Clear()
        For Each sDireccion As String In sDirecciones
            If sDireccion.Contains(vbCrLf) Then sDireccion = sDireccion.Replace(vbCrLf, "")
            Select Case True
                Case sDireccion.Contains(" ") : sDireccion = sDireccion.Replace(" ", "")
                Case sDireccion.Contains("\n") : sDireccion = sDireccion.Replace("\n", "")
                Case sDireccion.Contains("<") : sDireccion = sDireccion.Replace("<", "")
                Case sDireccion.Contains(">") : sDireccion = sDireccion.Replace(">", "")
            End Select

            If sDireccion <> "" Then ListView1.Items.Add(sDireccion)
        Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        CargarOpciones()
        ListView1.Columns(0).Width = ListView1.Width - 10

        txtCV.Text = sRutaCV
        txtCartaPresentacion.Text = sRutaCarta
        If txtCartaPresentacion.Text.Trim <> "" Then OpenCarta()

    End Sub

    Private Sub btnCV_Click(sender As Object, e As EventArgs) Handles btnCV.Click
        Dim oAbrir As New OpenFileDialog

        oAbrir.AddExtension = True
        oAbrir.Filter = "Archivos PDF|*.pdf|Todos los archivos|*.*"
        oAbrir.ShowDialog()

        txtCV.Text = oAbrir.FileName
    End Sub

    Private Sub OpenCarta()
        Dim documento As New Document()
        documento.LoadFromFile(txtCartaPresentacion.Text)
        TextBox1.Text = documento.GetText()
    End Sub

    Private Sub btnCartaPresentacion_Click(sender As Object, e As EventArgs) Handles btnCartaPresentacion.Click
        Dim oAbrir As New OpenFileDialog

        Try
            oAbrir.AddExtension = True
            oAbrir.Filter = "Documentos de texto|*.doc|Todos los archivos|*.*"
            oAbrir.ShowDialog()

            txtCartaPresentacion.Text = oAbrir.FileName

            ' Introducir en memoria la carta de presentación para el posterior envío
            If txtCartaPresentacion.Text.Trim <> "" Then OpenCarta()

        Catch excepcion As Exception
            Logger.e("Error de al abrir el documento:\n", excepcion)
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles mnuSalir.Click
        Application.Exit()
    End Sub

    Private Sub ConfiguracionToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ConfiguracionToolStripMenuItem1.Click

        frmConfig.ShowDialog()
        CargarOpciones()

    End Sub

    Private Sub CargarOpciones()
        ' Leer el archivo de Config.ini
        Dim FicheroIni As New IniFile(FICHEROINICIO)
        Dim Keys As ArrayList = FicheroIni.GetKeys("Propiedades email")


        For Each MiKey As Key In Keys

            Select Case MiKey.Name
                Case "Usuario" : Usuario = MiKey.Value
                Case "Pass" : Pass = MiKey.Value
                Case "Server" : Server = MiKey.Value
                Case "Port" : Port = MiKey.Value
                Case "SSL" : SSL = MiKey.Value
                Case "Credenciales" : Credenciales = MiKey.Value
                Case "Asunto" : sAsunto = MiKey.Value
                Case "Ruta CV" : sRutaCV = MiKey.Value
                Case "Ruta Carta" : sRutaCarta = MiKey.Value
            End Select

        Next

    End Sub
End Class