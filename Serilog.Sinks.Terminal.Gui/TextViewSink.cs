using NStack;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Reflection;
using Terminal.Gui;

namespace Serilog.Sinks.Terminal.Gui
{
    public class TextViewSink : ILogEventSink
    {
        private IFormatProvider Provider;
        private TextView View;
        private MethodInfo InsertTextMethod;

        public TextViewSink(TextView view, IFormatProvider provider = null)
        {
            Provider = provider;
            View = view;

            // We should not allow the TextView to be focused. Scrolling can cause problems.
            View.CanFocus = false;

            // Fetch the InsertText(ustring text) method. For whatever reason, there is no API for appending to the
            // TextView. "TextView.Text += newText" does work, but it doesn't scroll the TextView to the bottom
            // like I want it to. So, we're stuck with this gigantic hack for now.
            InsertTextMethod = view.GetType().GetMethod("InsertText", BindingFlags.Instance | BindingFlags.NonPublic,
                                                        null, new Type[] { typeof(ustring) }, null);
        }

        public void Emit(LogEvent logEvent)
        {
            string formattedMessage = logEvent.RenderMessage(Provider) + "\n";

            Application.MainLoop?.Invoke(() =>
            {
                InsertTextMethod.Invoke(View, new object[] { ustring.Make(formattedMessage) });
                View.SetNeedsDisplay();
            });
        }

    }
}
