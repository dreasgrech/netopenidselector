using System;

namespace OpenIDSelector
{
    /// <summary>
    /// Provides data for the SelectorChosen event
    /// </summary>
    public class ProviderChosenEventArgs : EventArgs
    {
        public string SelectorURL { get; set; }
        public ProviderChosenEventArgs(string provider)
        {
            SelectorURL = provider;
        }
    }
}
