using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidVectorViewer {
    
    class VectorCommand {
        private char mCommand;
        private string mPath;
        public void setCommand(char command ) {
            mCommand = command;
        }
        public void setPath(string path ) {
            mPath = path;
        }

        public char getCommand() { return mCommand; }
        public string getPath() { return mPath; }
    }


}
