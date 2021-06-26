using Serilog.Configuration;
using System;
using Terminal.Gui;

namespace Serilog.Sinks.Terminal.Gui
{
    public static class TextViewSinkExtensions
    {
        public static LoggerConfiguration TextViewSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  TextView textView,
                  IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new TextViewSink(textView, formatProvider));
        }

    }
}
