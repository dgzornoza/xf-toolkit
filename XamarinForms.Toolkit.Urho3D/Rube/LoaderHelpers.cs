using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Urho;
using Urho.Urho2D;
using XamarinForms.Toolkit.Helpers;

namespace XamarinForms.Toolkit.Urho3D.Rube
{
    /// <summary>
    /// Helpers for load rube files
    /// </summary>
    public static class LoaderHelpers
    {
        /// <summary>
        /// Helper for get JObject from rube json file.
        /// </summary>
        /// <param name="jsonFilePath">file path with physic world in b2djson format from R.U.B.E editor (relative to urho resources)</param>
        /// <returns>JObject with rube json file</returns>
        public static JObject GetJObjectFromJsonFile(string jsonFilePath)
        {
            string fullpath = Application.Current.ResourceCache.GetResourceFileName(jsonFilePath);
            string str = File.ReadAllText(fullpath);
            JObject result = JObject.Parse(str);

            return result;
        }

        /// <summary>
        /// Helper to load rube json object into urho3d node
        /// </summary>
        /// <param name="jsonObject">json object with physic world in b2djson format from R.U.B.E editor</param>
        /// <param name="urhoNode">Urho2d node where will be loaded</param>
        /// <param name="includeWorld">Flag for include world (true include world, false otherwise)</param>
        /// <param name="jsonFilePath">file path with R.U.B.E json file, only used for load images (relative to urho resources)</param>
        /// <returns>created B2dJson object with json data</returns>
        public static B2dJson ReadIntoNodeFromValue(JObject jsonObject, Urho.Node urhoNode, bool includeWorld, string jsonFilePath)
        {
            B2dJson b2dJson = new B2dJson();
            b2dJson.ReadIntoNodeFromValue(jsonObject, urhoNode, includeWorld);

            // crear los sprites existentes de las imagenes asociadas en el archivo rube
            _createUrhoSpritesFromImages(b2dJson, System.IO.Path.GetDirectoryName(jsonFilePath));

            return b2dJson;
        }

        /// <summary>
        /// Helper to load rube json string into urho3d node
        /// </summary>
        /// <param name="jsonString">string with physic world in b2djson format from R.U.B.E editor</param>
        /// <param name="urhoNode">Urho2d node where will be loaded</param>
        /// <param name="includeWorld">Flag for include world (true include world, false otherwise)</param>
        /// <param name="jsonFilePath">file path with R.U.B.E json file, only used for load images (relative to urho resources)</param>
        /// <returns>created B2dJson object with json data</returns>
        public static B2dJson ReadIntoNodeFromString(string jsonString, Urho.Node urhoNode, bool includeWorld, string jsonFilePath)
        {
            B2dJson b2dJson = new B2dJson();
            b2dJson.ReadIntoNodeFromString(jsonString, urhoNode, includeWorld);

            // crear los sprites existentes de las imagenes asociadas en el archivo rube
            _createUrhoSpritesFromImages(b2dJson, Path.GetDirectoryName(jsonFilePath));

            return b2dJson;
        }


        /// <summary>
        /// Helper to load rube json file into urho node.
        /// </summary>
        /// <param name="jsonFilePath">file path with physic world in b2djson format from R.U.B.E editor (relative to urho resources)</param>
        /// <param name="urhoNode">Urho2d node where will be loaded</param>
        /// <param name="includeWorld">Flag for include world (true include world, false otherwise)</param>
        /// <returns>created B2dJson object with json data</returns>
        public static B2dJson LoadRubeJson(string jsonFilePath, Urho.Node urhoNode, bool includeWorld)
        {
            string fullpath = Application.Current.ResourceCache.GetResourceFileName(jsonFilePath);

            B2dJson b2dJson = new B2dJson();
            b2dJson.ReadIntoNodeFromFile(fullpath, urhoNode, includeWorld);

            // crear los sprites existentes de las imagenes asociadas en el archivo rube
            _createUrhoSpritesFromImages(b2dJson, Path.GetDirectoryName(jsonFilePath));

            return b2dJson;
        }



        private static void _createUrhoSpritesFromImages(B2dJson b2dJson, string jsonFilePath)
        {
            // crear un vector con todas las imagenes de la escena del editor RUBE
            IEnumerable<B2dJsonImage> b2dImages = b2dJson.GetAllImages();
            var cache = Application.Current.ResourceCache;

            // recorrer el vector, crear los sprites para cada imagen y almacenarla en el array con imagenes asociadas a cuerpos fisicos
            foreach (var img in b2dImages)
            {
                // si la imagen no tiene un nodo asociado y el flag indica que no se cargue, se continua con la siguiente
                if (null == img.Body) continue;

                // probar a cargar la imagen del sprite, ignorar si falla
                string imageFilePath = PathHelpers.SimplifyPath(Path.Combine(jsonFilePath, img.File));
                Sprite2D sprite = cache.GetSprite2D(imageFilePath);
                if (sprite == null) continue;

                // añadir el sprite al nodo de fisicas y establecer el orden de renderizado
                StaticSprite2D staticSprite = img.Body.Node.CreateComponent<StaticSprite2D>();
                staticSprite.Sprite = sprite;
                staticSprite.OrderInLayer = (int)img.RenderOrder;

                // establecer propiedades del sprite
                staticSprite.FlipX = img.Flip;
                staticSprite.Color = Color.FromByteFormat((byte)img.ColorTint[0], (byte)img.ColorTint[1], (byte)img.ColorTint[2], (byte)img.ColorTint[3]);
                staticSprite.Alpha = img.Opacity;
                staticSprite.BlendMode = BlendMode.Alpha;
            }
        }
    }
}
