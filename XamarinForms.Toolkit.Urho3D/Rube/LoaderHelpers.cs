using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Urho;
using Urho.Urho2D;

namespace XamarinForms.Toolkit.Urho3D.Rube
{
    /// <summary>
    /// Helpers for load rube files
    /// </summary>
    public static class LoaderHelpers
    {
        /// <summary>
        /// Helper to load rube json file into urho3d node.
        /// </summary>
        /// <param name="jsonfilePath">file path with physic world in b2djson format from R.U.B.E editor (relative to urho resources)</param>
        /// <param name="urhoNode">Urho2d node where will be loaded</param>
        /// <param name="includeWorld">Flag for include world (true include world, false otherwise)</param>
        /// <returns>created B2dJson object with json data</returns>
        public static B2dJson LoadRubeJson(string jsonfilePath, Urho.Node urhoNode, bool includeWorld)
        {
            string fullpath = Application.Current.ResourceCache.GetResourceFileName(jsonfilePath);

            B2dJson b2dJson = new B2dJson();
            b2dJson.ReadIntoNodeFromFile(fullpath, urhoNode, includeWorld);

            // crear los sprites existentes de las imagenes asociadas en el archivo rube
            _createUrhoSpritesFromImages(b2dJson, Path.GetDirectoryName(jsonfilePath));

            return b2dJson;
        }



        private static void _createUrhoSpritesFromImages(B2dJson b2dJson, string jsonPath)
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
                string imagePath = Helpers.Path.SimplifyPath(Path.Combine(jsonPath, img.File));
                Sprite2D sprite = cache.GetSprite2D(imagePath);
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
