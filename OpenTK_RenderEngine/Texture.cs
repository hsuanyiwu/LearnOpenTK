using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_RenderEngine
{
    internal class Texture
    {
        private int _textureId;

        public Texture(int textureId)
        {
            this._textureId = textureId; 
        }

        public int TextureId
        {
            get {  return _textureId; }
        }

        public static Texture FromFile(string filePath)
        {
            return new Texture(Loader.CreateTexture(filePath));
        }
    }
}
