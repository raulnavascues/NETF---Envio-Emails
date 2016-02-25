Imports EnvioEmail.IniFile
Public Class frmConfig
    Dim lCambio As Boolean = False

    Private Const FICHEROINICIO As String = "Config.ini"
    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Dim FicheroIni As New IniFile(FICHEROINICIO)

        FicheroIni.AddKey("Usuario", txtUsuario.Text, "Propiedades email", True, True)
        FicheroIni.AddKey("Pass", txtPassword.Text, "Propiedades email", True, True)
        FicheroIni.AddKey("Server", txtServidor.Text, "Propiedades email", True, True)
        FicheroIni.AddKey("Port", txtPuerto.Text, "Propiedades email", True, True)
        FicheroIni.AddKey("SSL", chkHabilitarSSL.Checked, "Propiedades email", True, True)
        FicheroIni.AddKey("Credenciales", chkHabilitarCredenciales.Checked, "Propiedades email", True, True)
        FicheroIni.AddKey("Asunto", txtAsunto.Text, "Propiedades email", True, True)

        FicheroIni.AddKey("Ruta CV", txtRutaCV.Text, "Propiedades email", True, True)
        FicheroIni.AddKey("Ruta Carta", txtRutaCarta.Text, "Propiedades email", True, True)

        FicheroIni.Save(FICHEROINICIO) 'Save the file
        lCambio = False
        Me.Close()
    End Sub

    Private Sub frmConfig_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If lCambio Then
            If MessageBox.Show("Ha cambiado la configuración ¿Desea descartar los cambios?", "Configuración", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
                ' Cancelar el cerrado del formulario
                e.Cancel = True

            End If

        End If
    End Sub

    Private Sub frmConfig_Load(sender As Object, e As EventArgs) Handles Me.Load
        CargarOpciones()
        lCambio = False
    End Sub

    Private Sub CargarOpciones()
        ' Leer el archivo de Config.ini
        Dim FicheroIni As New IniFile(FICHEROINICIO)
        Dim Keys As ArrayList = FicheroIni.GetKeys("Propiedades email")


        For Each MiKey As Key In Keys

            Select Case MiKey.Name
                Case "Usuario" : txtUsuario.Text = MiKey.Value
                Case "Pass" : txtPassword.Text = MiKey.Value
                Case "Server" : txtServidor.Text = MiKey.Value
                Case "Port" : txtPuerto.Text = MiKey.Value
                Case "SSL" : chkHabilitarSSL.Checked = MiKey.Value
                Case "Credenciales" : chkHabilitarCredenciales.Checked = MiKey.Value
                Case "Asunto" : txtAsunto.Text = MiKey.Value
                Case "Ruta CV" : txtRutaCV.Text = MiKey.Value
                Case "Ruta Carta" : txtRutaCarta.Text = MiKey.Value
            End Select

        Next
    End Sub

    Private Sub txtUsuario_TextChanged(sender As Object, e As EventArgs) Handles txtUsuario.TextChanged, txtPassword.TextChanged, txtPuerto.TextChanged, txtServidor.TextChanged, chkHabilitarCredenciales.CheckedChanged, chkHabilitarSSL.CheckedChanged, txtAsunto.TextChanged, txtRutaCV.TextChanged, txtRutaCarta.TextChanged

        lCambio = True

    End Sub
End Class