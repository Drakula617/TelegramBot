using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using Word = Microsoft.Office.Interop.Word;

using Xceed.Document.NET;
using Xceed.Words.NET;
using Microsoft.Office.Interop.Word;
using Telegram.Bot.Types.Enums;
using System.Reflection.Metadata;
using Task = System.Threading.Tasks.Task;
using Xceed.Pdf.Layout.Shape;
using Path = System.IO.Path;

namespace TG_Bot2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string token = "5609768899:AAHETKwCfKg7clUx2Lw3pMQaooMEOuLMJkQ";
            var client = new TelegramBotClient(token);

            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }
        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
        public static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            Message message = update.Message;

            if (message == null)
                return;
            else if (message.Type != MessageType.Text)
            {

                DirectoryInfo dir = new DirectoryInfo(@"C:\Users\gidin\Desktop\TG_FIle\User_of_Files");
                string ChatName = message.Chat.Username.ToString() + "_" + message.Chat.Id.ToString();
                if (dir.GetDirectories().ToList().Select(d => new
                {
                    NameFolder = d.Name.ToString()
                }).ToList().Where(c => c.NameFolder == ChatName).Count() == 0)
                {
                    Directory.CreateDirectory($"{dir.FullName}\\{ChatName}");
                }
                dir = new DirectoryInfo($"{dir.FullName}\\{ChatName}");

                //if (message.Type.ToString() != "Text")
                //{
                //await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: message.Type.ToString());
                //var document = message.Document;
                string typeMessage = message.Type.ToString();

                //var file = botClient.GetFileAsync(message..FileId);
                //string fileName = document.FileName;
                //DirectoryInfo dirUser = new DirectoryInfo(@"C:\Users\gidin\Desktop\TG_FIle\User_of_Files" + "\\" + ChatName);
                //if (file.Result == null)
                //    return;
                //else
                //{


                string filename = string.Empty;
                if (dir.GetDirectories().ToList().Select(d => new
                {
                    NameFolder = d.Name.ToString()
                }).ToList().Where(c => c.NameFolder == typeMessage).Count() == 0)
                {
                    Directory.CreateDirectory(dir.FullName + "\\" + typeMessage);
                }
                dir = new DirectoryInfo(dir.FullName + "\\" + typeMessage);
                //
                Telegram.Bot.Types.File file = new Telegram.Bot.Types.File();
                //Image image;
                string filePath = String.Empty;
                string extFile = String.Empty;
                if (message.Type == MessageType.Document)
                {
                    file = await botClient.GetFileAsync(message.Document.FileId);
                    //dir = new DirectoryInfo(dir.FullName + "\\" + extFile);  
                    filePath = file.FilePath;
                    extFile = Path.GetExtension(filePath);
                    filename = message.Document.FileName;
                }
                else if (message.Type == MessageType.Photo)
                {
                   file = await botClient.GetFileAsync(message.Photo[message.Photo.Count() - 1].FileId);
                   filename = Path.GetFileName(file.FilePath);
                }
                else if (message.Type == MessageType.Video)
                {
                    file = await botClient.GetFileAsync(message.Video.FileId);
                    filename = message.Video.FileName;
                }
                else if (message.Type == MessageType.Audio)
                {
                    file = await botClient.GetFileAsync(message.Audio.FileId);
                    filePath = file.FilePath;
                    extFile = Path.GetExtension(filePath);
                    filename = message.Audio.FileName;
                }
                filePath = file.FilePath;
                if (extFile != string.Empty)
                {
                    if (dir.GetDirectories().ToList().Select(d => new
                    {
                        NameFolder = d.Name.ToString()
                    }).ToList().Where(c => c.NameFolder == extFile).Count() == 0)
                    {
                        Directory.CreateDirectory(dir.FullName + "\\" + extFile);
                    }
                    dir = new DirectoryInfo(dir.FullName + "\\" + extFile);
                }

                if (File.Exists(Path.GetFileName(filePath)))
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Такой файл есть в вашем каталоге");
                }
                else
                {
                    var FilePath = Path.Combine(dir.FullName ,filename);
                    await using var fs = new FileStream(FilePath, FileMode.Create);
                    await botClient.DownloadFileAsync(filePath: file.FilePath, fs);
                }
                //}
                //}
                //else if (message.Type.ToString() == "Text")
                //{
                //    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Текст");
                //}

            }
        }
    
    }
}
