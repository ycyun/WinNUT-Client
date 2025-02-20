﻿' WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Imports System.IO

Public Module WinNUT_Globals

#Region "Constants/Shareds"

    Public ReadOnly ProgramName = My.Application.Info.ProductName
    Public ReadOnly LongProgramName = My.Application.Info.Description
    Public ReadOnly ProgramVersion = My.Application.Info.Version.ToString()
    Public ReadOnly ShortProgramVersion = ProgramVersion.Substring(0, ProgramVersion.IndexOf(".", ProgramVersion.IndexOf(".") + 1))
    Public ReadOnly GitHubURL = My.Application.Info.Trademark
    Public ReadOnly Copyright = My.Application.Info.Copyright

#Region "File Directories"

    Private ReadOnly DATA_DIRECTORY_NAME = "WinNUT-Client"
    Private ReadOnly TEMP_FOLDER = Path.GetTempPath() + DATA_DIRECTORY_NAME

#If DEBUG Then
    Public ReadOnly IsDebugBuild = True
    ' If debugging, keep any generated data next to the debug executable.
    Private ReadOnly DESIRED_DATA_PATH As String = Path.Combine(Environment.CurrentDirectory, DATA_DIRECTORY_NAME)
#Else
    Public ReadOnly IsDebugBuild = False
    Private ReadOnly DESIRED_DATA_PATH As String = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), DATA_DIRECTORY_NAME)
#End If

    Public ReadOnly TEMP_DATA_PATH = Path.Combine(Path.GetTempPath(), DATA_DIRECTORY_NAME)

#End Region

    Public ApplicationDataPath = TEMP_DATA_PATH
    Public WithEvents LogFile As Logger = New Logger(LogLvl.LOG_DEBUG)
    Public StrLog As New List(Of String)

#End Region

    Public Sub Init_Globals()
        ApplicationDataPath = GetAppDirectory(DESIRED_DATA_PATH)
    End Sub

    ''' <summary>
    ''' Do everything possible to find a safe place to write to, with the <see cref="ProgramName"/> appended to it. If
    ''' the requested option is unavailable, we fall back to the temporary directory for the current user.
    ''' </summary>
    ''' <param name="requestedDir">The requested directory, with <see cref="ProgramName"/> appended to it.</param>
    ''' <returns>The best possible option available as a writable data directory.</returns>
    Private Function GetAppDirectory(requestedDir As String) As String
        Dim finalDir As String

        Try
            Directory.CreateDirectory(requestedDir)
            LogFile.LogTracing("Successfully created or opened requested data directory for WinNUT." &
                               vbNewLine & "requestedDir: " & requestedDir, LogLvl.LOG_DEBUG, Nothing)
            finalDir = requestedDir

        Catch ex As Exception
            LogFile.LogTracing(ex.ToString & " encountered trying to create app data directory. Falling back to temp.",
                               LogLvl.LOG_ERROR, Nothing)

            Directory.CreateDirectory(TEMP_DATA_PATH)
            finalDir = TEMP_DATA_PATH
        End Try

        Return finalDir
    End Function
End Module
