using System;

namespace OpenIDSelector
{
    /// <summary>
    /// Provides data for the SelectorChosen event
    /// </summary>
    public class ProviderChosenEventArgs : EventArgs
    {
        public ProviderChosenEventArgs(string provider)
        {
            SelectorURL = provider;
        }

        public string SelectorURL { get; set; }
    }
}
