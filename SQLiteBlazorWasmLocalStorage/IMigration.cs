// <copyright file="IMigration.cs" company="VeriNova Technologies Inc.">
// Copyright (c) VeriNova Technologies Inc.. All rights reserved.
// </copyright>

namespace SQLiteBlazorWasmLocalStorage
{
    /// <summary>
    /// Determines whether to use the migration strategy or ensure created.
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Reads the prefered entity framework database creation strategy.
        /// </summary>
        /// <returns>True to use migration. False to use ensure created.</returns>
        bool UseMigration();
    }
}
