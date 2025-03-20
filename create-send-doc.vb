Function GetDownloadsFolder() As String
    Dim downloadsPath As String

    #If Mac Then
        username = Environ("USER")
        downloadsPath = "/Users/" & username & "/Downloads/"
    #Else
        Dim WshShell As Object
        Set WshShell = CreateObject("WScript.Shell")
        downloadsPath = WshShell.ExpandEnvironmentStrings("%USERPROFILE%") & "\Downloads\"
    #End If

    GetDownloadsFolder = downloadsPath
End Function

Sub DeleteAnalytics()
    Application.ScreenUpdating = False
    Application.DisplayAlerts = False

    With ActiveDocument.Content.Find
        .ClearFormatting
        .Style = "Analytic"
        .Text = ""
        .Replacement.Text = ""
        .Forward = True
        .Wrap = wdFindContinue
        .Execute Replace:=wdReplaceAll
    End With

    With ActiveDocument.Content.Find
        .ClearFormatting
        .Style = "Undertag"
        .Text = ""
        .Replacement.Text = ""
        .Forward = True
        .Wrap = wdFindContinue
        .Execute Replace:=wdReplaceAll
    End With
End Sub

Sub CreateSendDoc()
    Dim newDoc As Document
    Dim savePath As String
    Dim downloadsDirPath As String
    Dim sendDoc As Document
    Dim originalDoc As Document

    set originalDoc = ActiveDocument

    Set sendDoc = Documents.Add(ActiveDocument.FullName)

    ' Process the document to remove analytics content
    Call DeleteAnalytics()

    ' Get the Downloads folder path
    downloadsDirPath = GetDownloadsFolder()
    savePath = downloadsDirPath & "[S] " & originalDoc.Name
    ActiveDocument.SaveAs2 Filename:=savePath, FileFormat:=wdFormatDocumentDefault
    sendDoc.Close
End Sub
