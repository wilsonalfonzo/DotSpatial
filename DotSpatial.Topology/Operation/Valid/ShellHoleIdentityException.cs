﻿// ********************************************************************************************************
// Product Name: DotSpatial.Topology.Operation.Valid.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code was written by Ted Dunsford 6/23/2010 4:08:52 PM for the DotSpatial.Topology.Operation.Valid library.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.Operation.Valid
{
    /// <summary>
    /// A ShellHoleIdentityException Class
    /// </summary>
    public class ShellHoleIdentityException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of ShellHoleIdentityException
        /// </summary>
        public ShellHoleIdentityException()
            : base(TopologyText.ShellHoleIdentityException)
        {
        }

        #endregion
    }
}