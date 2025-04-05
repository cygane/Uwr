using System;
using System.IO;
using Xunit;

public class CaesarStreamTests
{
    [Fact]
    public void WriteAndRead_ShouldReturnOriginalData_WhenShiftedBack()
    {
        string testText = "Hello Caesar!";
        byte[] originalData = System.Text.Encoding.UTF8.GetBytes(testText);
        byte[] readBuffer = new byte[originalData.Length];

        using (var memoryStream = new MemoryStream())
        {
            using (var writeStream = new CaesarStream(memoryStream, 5, leaveOpen: true))
            {
                writeStream.Write(originalData, 0, originalData.Length);
                writeStream.Flush();
            }

            memoryStream.Position = 0;

            using (var readStream = new CaesarStream(memoryStream, -5, leaveOpen: true))
            {
                readStream.Read(readBuffer, 0, readBuffer.Length);
            }
        }

        string resultText = System.Text.Encoding.UTF8.GetString(readBuffer);
        Assert.Equal(testText, resultText);
    }
}

