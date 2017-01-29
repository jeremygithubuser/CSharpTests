using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace CSharpTests.Streams
{
    [TestClass]
    public class StreamTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void WriteToFile()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            Byte[] Bytes = new byte[26];

            for (int i = 0; i < alphabet.Length; i++)
            {
                Bytes[i] = Convert.ToByte(alphabet[i]);
            }

            TestContext.WriteLine($"HEX VAL");
            for (int i = 0; i < Bytes.Length; i++)
            {
                TestContext.WriteLine($"{alphabet[i]} : {Bytes[i]:x2}");
            }


            TestContext.WriteLine($"BINARY VAL");
            for (int i = 0; i < Bytes.Length; i++)
            {
                TestContext.WriteLine($"{alphabet[i]} : {Convert.ToString(Bytes[i], 2).PadLeft(8, '0')}");
            }

            using (FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write))
            {
                fs.Write(Bytes, 0, Bytes.Length);
                ;
                var intArray = new[] { 0, 0, 1, 0, 0, 0, 1, 1 }.Reverse().ToArray();
                BitArray bits = new BitArray(8);//(intArray);
                bits[0] = true;
                bits[1] = true;
                bits[2] = false;
                bits[3] = false;
                bits[4] = false;
                bits[5] = true;
                bits[6] = false;
                bits[7] = false;
                var bytes = new byte[bits.Length];
                bits.CopyTo(bytes, 0);
                fs.Write(bytes, 0, 1);
            }

        }

        [TestMethod]
        public void PrintImageArray()
        {

            var byteArray = File.ReadAllBytes("input.bmp");

            for (int i = 0; i < byteArray.Length; i++)
            {
                TestContext.WriteLine($"{i} : {Convert.ToString(byteArray[i], 2).PadLeft(8, '0')}");
            }


        }
    }
}
