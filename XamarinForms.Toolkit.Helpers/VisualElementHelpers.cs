using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace XamarinForms.Toolkit.Helpers
{
	/// <summary>
    /// Clase con un modelo representando las propiedades visuales de un elemento xaml
    /// </summary>
    public class VisualElementPropertiesModel
    {
        /// <summary>Propiedad con la posicion X</summary>
        public double X { get; set; }
        /// <summary>Propiedad con la posicion Y</summary>
        public double Y { get; set; }

        /// <summary>Propiedad con la posicion escala</summary>
        public double Scale { get; set; }

        /// <summary>Propiedad con la rotacion Z</summary>
        public double Rotation { get; set; }
        /// <summary>Propiedad con la rotacion X</summary>
        public double RotationX { get; set; }
        /// <summary>Propiedad con la rotacion Y</summary>
        public double RotationY { get; set; }

        /// <summary>Propiedad con el punto de anclaje X</summary>
        public double AnchorX { get; set; }
        /// <summary>Propiedad con el punto de anclaje Y</summary>
        public double AnchorY { get; set; }

        /// <summary>Propiedad con el ancho</summary>
        public double Width { get; set; }
        /// <summary>Propiedad con el alto</summary>
        public double Height { get; set; }

        /// <summary>Propiedad con la opacidad</summary>
        public double Opacity { get; set; }
    }

	
    /// <summary>
    /// Helpers para trabajar con el arbol visual xaml
    /// </summary>
    public static class VisualElementHelpers
    {
		/// <summary>
        /// Metodo extensor para obtener las propiedades de un elemento visual
        /// </summary>
        /// <param name="element">Elemento visual del cual seran obtenidas las propiedades</param>
        /// <returns>Propiedades del elemento visual</returns>
        public static VisualElementPropertiesModel GetVisualElementProperties(this VisualElement element)
        {

            return new VisualElementPropertiesModel
            {
                X = element.TranslationX,
                Y = element.TranslationY,
                Scale = element.Scale,
                Rotation = element.Rotation,
                RotationX = element.RotationX,
                RotationY = element.RotationY,
                AnchorX = element.AnchorX,
                AnchorY = element.AnchorY,
                Width = element.Width,
                Height = element.Height,
                Opacity = element.Opacity
            };
        }
		
		/// <summary>
        /// Metodo extensor para establecer propiedades de un elemento visual
        /// </summary>
        /// <param name="element">Elemento visual en el cual seran establecidas las propiedades</param>
        /// <param name="properties">Propiedades a establecer en el elemento visual</param>
        public static void SetVisualElementProperties(this VisualElement element, VisualElementPropertiesModel properties)
        {            
            element.TranslationX = properties.X;
            element.TranslationY = properties.Y;
            element.Scale = properties.Scale;
            element.Rotation = properties.Rotation;
            element.RotationX = properties.RotationX;
            element.RotationY = properties.RotationY;
            element.AnchorX = properties.AnchorX;
            element.AnchorY = properties.AnchorY;
            element.WidthRequest = properties.Width;
            element.HeightRequest = properties.Height;
            element.Opacity = properties.Opacity;
        }
		
		/// <summary>
        /// Funcion para intercambiar posicion de los hijos especificados en un Layout padre que permita hijos
        /// </summary>
        /// <param name="parent">Elemento padre</param>
        /// <param name="element1">elemento 1 a intercambiar por el 2</param>
        /// <param name="element2">elemento 2 a intercambiar por el 1</param>        
        public static void SwapChildrens(this Grid parent, VisualElement element1, VisualElement element2)
        {
            View child1 = element1 as View;
            View child2 = element2 as View;
            if (null == child1 || null == child2 || child1 == child2) return;
            

            try
            {
                // Se clona la lista por que no se puede modificar directamente una coleccion observable
                List<View> childs = new List<View>(parent.Children);
                int index1 = childs.IndexOf(child1);
                int index2 = childs.IndexOf(child2);
                childs[index1] = child2;
                childs[index2] = child1;

                parent.Children.Clear();
                foreach (var item in childs) parent.Children.Add(item);
            }
            catch (Exception ex)
            {
                LogHelpers.RegisterLog(ex.GetExceptionStringFormat(), LogHelpers.EnumWarningLevel.LEVEL_5);
            }
         
        }
		
        /// <summary>
        /// Funcion para buscar el primer control padre del tipo especificado al que pertenece el elemento visual
        /// </summary>
        /// <param name="element">Elemento del cual sera obtenido el padre</param>
        /// <returns>El primer control padre del tipo especificado, null si no se encuentra</returns>
        public static T FindVisualAncestor<T>(this VisualElement element) where T : VisualElement
        {
            T result = default(T);
            if (null != element)
            {
                var parent = element.Parent;
                while (parent != null)
                {
                    if (parent is T)
                    {
                        return parent as T;
                    }
                    parent = parent.Parent;
                }
            }
            return result;
        }

        /// <summary>
        /// Funcion recursiva para buscar elementos visuales hijos en un elemento padre
        /// </summary>
        /// <typeparam name="T">Tipo del hijo visual a buscar</typeparam>
        /// <param name="parentElement">Elemento visual padre</param>
        /// <param name="whereSearch">Elemento visual interno donde buscar</param>
        /// <param name="containsStringName">(opcional) nombre del elemento hijo a buscar</param>
        /// <param name="result">(opcional) Lista de elmeentos donde añadir los elementos encontrados</param>
        /// <returns>Elementos hijos encontrados</returns>
        public static List<T> FindVisualChildren<T>(this VisualElement parentElement, VisualElement whereSearch, string containsStringName = null, List<T> result = null)
        {
            result = result ?? new List<T>();

            try
            {
                var props = whereSearch.GetType().GetRuntimeProperties();
                var contentProp = props.FirstOrDefault(w => w.Name == "Content");
                var childProp = props.FirstOrDefault(w => w.Name == "Children");
                var itemsProp = props.FirstOrDefault(w => w.Name == "TemplatedItems");
                if (childProp == null) childProp = itemsProp;

                // el padre es un contenedor
                if (childProp == null && contentProp != null && contentProp.GetValue(whereSearch) is VisualElement cv)
                {
                    FindVisualChildren<T>(parentElement, cv, containsStringName, result);
                    return result;
                }
                
                // cualquier tipo de elemento padre que no es un contenedor
                IEnumerable values = childProp.GetValue(whereSearch) as IEnumerable;
                foreach (var value in values)
                {
                    var tempValue = value;
                    if (tempValue is ViewCell) tempValue = ((ViewCell)tempValue).View;
                    if (tempValue is VisualElement) FindVisualChildren<T>(parentElement, tempValue as VisualElement, containsStringName, result);

                    if (tempValue is T)
                    {
                        if (!string.IsNullOrEmpty(containsStringName))
                        {
                            bool check = false;
                            var fields = parentElement.GetType().GetRuntimeFields().Where(w => w.Name.ToLower().Contains(containsStringName.ToLower())).ToList();
                            foreach (var field in fields)
                            {
                                var fieldValue = field.GetValue(parentElement);
                                if (fieldValue is T && fieldValue == tempValue) { check = true; break; }
                            }
                            if (!check) continue;
                        }

                        result.Insert(0, (T)tempValue);
                    }
                }
                return result;
            }
            catch
            {
                return result;
            }
        }


    }
}
