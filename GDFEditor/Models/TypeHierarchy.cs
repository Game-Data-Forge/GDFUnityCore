using System;
using System.Collections.Generic;

namespace GDFEditor
{
    /// <summary>
    /// Represents a hierarchy of types, where each type can have child types forming a tree structure.
    /// </summary>
    public class TypeHierarchy
    {
        /// <summary>
        /// Represents a specific <see cref="Type"/> that is part of the type hierarchy.
        /// </summary>
        /// <remarks>
        /// The <see cref="Type"/> stored in this variable is utilized to represent the type within
        /// the <see cref="GDFEditor.TypeHierarchy"/> and its associated structures.
        /// </remarks>
        public Type type;

        /// <summary>
        /// Represents a list of child <see cref="TypeHierarchy"/> instances.
        /// Each child node can represent a subtype or hierarchical relationship within the tree structure.
        /// </summary>
        public List<TypeHierarchy> children;

        /// <summary>
        /// Represents a type hierarchy structure, containing a type and its child type hierarchies.
        /// </summary>
        public TypeHierarchy(Type type)
        {
            this.type = type;
            children = new List<TypeHierarchy>();
        }
    }
}
