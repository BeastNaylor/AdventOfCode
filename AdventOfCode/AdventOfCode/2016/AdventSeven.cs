using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class AdventSeven
    {
        public static void Run()
        {
            var addresses = "";
            var validABBAAddresses = 0;
            var validSSLAddresses = 0;
            int total = 0;
            foreach (string address in addresses.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var analyser = new AddressAnalyser(address);
                analyser.ProcessAddress();
                if (analyser.IsABBAValid)
                {
                    validABBAAddresses++;
                }
                if (analyser.IsSSLValid)
                {
                    validSSLAddresses++;
                }
                total++;
            }
            Console.WriteLine(String.Format("There are {0} valid ABBA addresses", validABBAAddresses));
            Console.WriteLine(String.Format("There are {0} valid SSL addresses", validSSLAddresses));
            Console.WriteLine(String.Format("There are {0} addresses", total));
        }


        private class AddressAnalyser
        {
            private string _address;
            private bool _hasABBA;
            private bool _hasABBAInBraces;

            private List<string> _sslCodes;
            private List<string> _sslCodeInBraces;

            public bool IsABBAValid
            {
                get
                {
                    return _hasABBA && !_hasABBAInBraces;
                }
            }

            public bool IsSSLValid
            {
                get
                {
                    var valid = false;
                    if (_sslCodes.Any() && _sslCodeInBraces.Any())
                    {
                        var matches = _sslCodes.Intersect(_sslCodeInBraces);
                        Console.WriteLine(String.Format("There are {0} matches: {1}", matches.Count(), String.Join(",", matches)));
                        valid = matches.Any();
                    }
                    return valid;
                }
            }


            public AddressAnalyser(string address)
            {
                _sslCodes = new List<string>();
                _sslCodeInBraces = new List<string>();
                _address = address;
            }

            //for each address, we need to determine if it has ABBA
            //for each character, check if the next two are the same. the check if the one after that is the same as the current one
            public void ProcessAddress()
            {
                var withinBraces = false;
                for (int ii = 0; ii <= _address.Length - 3; ii++)
                {
                    var firstChar = _address[ii];
                    var secondChar = _address[ii + 1];
                    var thirdChar = _address[ii + 2];
                    int fourthChar = 0;
                    if (ii + 3 <= _address.Length - 1) { fourthChar = _address[ii + 3]; }
                     

                    if (firstChar == '[') { withinBraces = true; }
                    if (firstChar == ']') { withinBraces = false; }

                    // if we have an ABBA formation, then we need to check if it is in the brackets. it can NEVER been valid if within the brackets
                    if (firstChar == fourthChar && secondChar == thirdChar && firstChar != secondChar)
                    {
                        if (withinBraces)
                        {
                            //Console.WriteLine("IN BRACES");
                            _hasABBAInBraces = true;
                        }
                        else
                        {
                            //Console.WriteLine("WITHOUT BRACES");
                            _hasABBA = true;
                        }
                    }

                    if (firstChar == thirdChar && firstChar != secondChar)
                    {
                        //if we have an ABA format, we need to save the format, along with whether it was within the brackets
                        //SSL is then valid if we have the reverse within the brackets as without
                        if (withinBraces)
                        {
                            _sslCodeInBraces.Add(firstChar.ToString() + secondChar.ToString());
                        }
                        else
                        {
                            _sslCodes.Add(secondChar.ToString() + firstChar.ToString());

                        }
                    }
                }
            }
        }
    }
}
