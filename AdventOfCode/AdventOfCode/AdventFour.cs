using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class AdventFour
    {
        public static void Run()
        {
            var rooms = Properties.Resources.AdventFour;
            var totalRoomID = 0;
            foreach (string fullRoom in rooms.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var room = new Room(fullRoom);

                room.DetermineCheckSum();
                if (room.IsValid)
                {
                    room.DecryptName();
                    totalRoomID += room.RoomID;
                }
            }
            Console.WriteLine("Total of Room IDs: {0}", totalRoomID);
        }

        private class Room
        {
            private int _roomID;
            private string _checkSum;
            private string _calculatedCheckSum;
            private string _encryptedRoomName;
            private string _decryptedRoomName;

            public Room(string encryptedName)
            {
                //Console.WriteLine(String.Format("The Original Room is :{0}", encryptedName));
                ParseName(encryptedName);
            }

            public int RoomID
            {
                get { return _roomID; }
            }

            //check if our calculated checksum matches the one given
            public bool IsValid
            {
                get
                {
                    return _checkSum == _calculatedCheckSum;
                }
            }

            private void ParseName(string encryptedName)
            {
                //parse out the different bits of the room name
                //first, get the [checksum]
                var regExp = "\\[([^[]+)\\]";
                Match result = Regex.Match(encryptedName, regExp);
                _checkSum = result.Value;
                //then take all the numbers from the room and save into the ID
                _roomID = Int32.Parse(Regex.Replace(encryptedName, "[^0-9]", ""));
                //then trim the checksum off the end, and save the encrpyted name
                _encryptedRoomName = encryptedName.Replace(_checkSum, "").Replace(_roomID.ToString(), "");
            }

            internal void DetermineCheckSum()
            {
                //trim out only the letters
                var onlyLettersRoomName = Regex.Replace(_encryptedRoomName, "[^a-zA-Z]", "");
                var orderedLetters = onlyLettersRoomName.GroupBy(x => x).OrderByDescending(x => x.Count());
                var checkSumContents = new List<IGrouping<char, char>>();
                var prevCount = 0;
                //need to go through each group and determine which are the most common letters. if there is a tie for any places we need to add both
                foreach (IGrouping<char, char> letter in orderedLetters)
                {
                    var count = letter.Count();
                    if (checkSumContents.Count < 5 || count == prevCount)
                    {
                        checkSumContents.Add(letter);
                    }
                    prevCount = count;
                }
                //then we can order by the count, and break any ties alphabetically
                var commonLetters = String.Join("", checkSumContents.OrderByDescending(x => x.Count()).ThenBy(x => x.Key.ToString()).Take(5).Select(x => x.Key));
                //Console.WriteLine(String.Format("The 5 most common letters are {0}", commonLetters));
                _calculatedCheckSum = String.Format("[{0}]", commonLetters);
            }

            internal void DecryptName()
            {
                //to decrypt the name, we rotate all the letters by the RoomID
                var shift = _roomID % 26;
                _decryptedRoomName = ShiftLetters(_encryptedRoomName, shift);
                //Console.WriteLine(String.Format("The decrypted name is {0}", _decryptedRoomName));
                if (_decryptedRoomName.StartsWith("northpole-object"))
                {
                    Console.WriteLine(String.Format("The RoomID for NorthPole Object Storage is {0}", _roomID));
                }
            }

            private string ShiftLetters(string value, int shift)
            {
                char[] buffer = value.ToCharArray();
                for (int i = 0; i < buffer.Length; i++)
                {
                    // Letter.
                    char letter = buffer[i];
                    if (letter >= 97 && letter <= 122)
                    {
                        // Add shift to all.
                        letter = (char)(letter + shift);
                        // Subtract 26 on overflow.
                        // Add 26 on underflow.
                        if (letter > 'z')
                        {
                            letter = (char)(letter - 26);
                        }
                        else if (letter < 'a')
                        {
                            letter = (char)(letter + 26);
                        }
                    }
                    // Store.
                    buffer[i] = letter;
                }
                return new string(buffer);
            }
        }
    }
}
