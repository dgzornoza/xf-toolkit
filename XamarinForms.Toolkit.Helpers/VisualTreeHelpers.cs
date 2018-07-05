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
    /// Helpers para trabajar con el arbol visual xaml
    /// </summary>
    public static class VisualTreeHelpers
    {

        /// <summary>
        /// Funcion para buscar el control con la pagina a la que pertenece el elemento visual
        /// </summary>
        /// <returns>El control con la pagina, null si no se encuentra</returns>
        /// <param name="element">Elemento hijo de la pagina a obtener</param>
        public static Page FindParentPage(this VisualElement element)
        {
            if (element != null)
            {
                var parent = element.Parent;
                while (parent != null)
                {
                    if (parent is Page)
                    {
                        return parent as Page;
                    }
                    parent = parent.Parent;
                }
            }
            return null;
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
