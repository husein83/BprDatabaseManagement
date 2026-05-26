using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Data.SqlClient;
using DatabaseManagement.Models;

namespace DatabaseManagement.Utility
{
    public static class Extensions
    {
        #region ProcedureExtensions
        public static void AddParameters(this IProcedure procedure, SqlCommand cmd)
        {
            var type = procedure.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                // Skip IProcedure methods
                if (prop.DeclaringType == typeof(IProcedure))
                    continue;

                var value = prop.GetValue(procedure);

                // Skip null values for nullable types
                if (value == null)
                    continue;

                var paramName = $"@{prop.Name}";
                cmd.Parameters.AddWithValue(paramName, value);
            }
        }

        public static Dictionary<string, object?> GetParameters(this IProcedure procedure)
        {
            var parameters = new Dictionary<string, object?>();
            var type = procedure.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.DeclaringType == typeof(IProcedure))
                    continue;

                var value = prop.GetValue(procedure);
                if (value != null)
                    parameters[$"@{prop.Name}"] = value;
            }

            return parameters;
        }

        public static void Validate(this IProcedure procedure)
        {
            var type = procedure.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (prop.DeclaringType == typeof(IProcedure))
                    continue;

                // Check required properties (non-nullable reference types)
                var nullabilityContext = new NullabilityInfoContext();
                var nullability = nullabilityContext.Create(prop);

                if (nullability.WriteState == NullabilityState.NotNull)
                {
                    var value = prop.GetValue(procedure);
                    if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        throw new ArgumentException($"Property '{prop.Name}' is required.", prop.Name);
                    }
                }
            }
        }

        #endregion

        #region ControlExtensions
        public static bool TryGetValue<TValue>
            (
                this Control control,
                out TValue value,
                string? validationMessage = null,
                string validationTitle = "Validation"
            )
        {
            value = default!;

            try
            {
                object? rawValue = null;

                if (control.Visible && control.Enabled)
                {
                    switch (control)
                    {
                        case TextBox textBox:
                            rawValue = textBox.Text;
                            break;

                        case ComboBox comboBox:
                            if (comboBox.SelectedItem is string comboBoxText && comboBoxText == Utility.Constants.COMBOBOX__EMPTY_VALUE_KEY)
                            {
                                rawValue = null;
                                break;
                            }
                            rawValue = comboBox.SelectedItem?.ToString();
                            break;

                        case NumericUpDown numericUpDown:
                            rawValue = numericUpDown.Value;
                            break;

                        case CheckBox checkBox:
                            switch (checkBox.CheckState)
                            {
                                case CheckState.Unchecked:
                                    rawValue = false;
                                    break;
                                case CheckState.Checked:
                                    rawValue = true;
                                    break;
                                case CheckState.Indeterminate:
                                    rawValue = null;
                                    break;
                            }
                            break;

                        case RadioButton radioButton:
                            rawValue = radioButton.Checked;
                            break;

                        case DateTimePicker dateTimePicker:
                            rawValue = dateTimePicker.Value;
                            break;

                        case ListBox listBox:
                            rawValue = listBox.SelectedItem?.ToString();
                            break;

                        case RichTextBox richTextBox:
                            rawValue = richTextBox.Text;
                            break;

                        case MaskedTextBox maskedTextBox:
                            rawValue = maskedTextBox.Text;
                            break;

                        default:
                            return false;
                    }
                }

                // Validation for string types
                if (typeof(TValue) == typeof(string))
                {
                    string? stringValue = rawValue?.ToString();

                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        if (validationMessage != null)
                        {
                            MessageBox.Show(validationMessage, validationTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        return false;
                    }

                    value = (TValue)(object)stringValue;
                    return true;
                }

                // Validation for nullable types
                if (rawValue == null)
                {
                    if (validationMessage != null)
                    {
                        MessageBox.Show(validationMessage, validationTitle,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return false;
                }

                // Type conversion
                if (typeof(TValue) == typeof(int))
                {
                    value = (TValue)(object)Convert.ToInt32(rawValue);
                }
                else if (typeof(TValue) == typeof(decimal))
                {
                    value = (TValue)(object)Convert.ToDecimal(rawValue);
                }
                else if (typeof(TValue) == typeof(bool))
                {
                    value = (TValue)(object)Convert.ToBoolean(rawValue);
                }
                else if (typeof(TValue) == typeof(DateTime))
                {
                    value = (TValue)(object)Convert.ToDateTime(rawValue);
                }
                else if (typeof(TValue) == typeof(double))
                {
                    value = (TValue)(object)Convert.ToDouble(rawValue);
                }
                else if (typeof(TValue) == typeof(float))
                {
                    value = (TValue)(object)Convert.ToSingle(rawValue);
                }
                else if (typeof(TValue) == typeof(long))
                {
                    value = (TValue)(object)Convert.ToInt64(rawValue);
                }
                else if (typeof(TValue) == typeof(short))
                {
                    value = (TValue)(object)Convert.ToInt16(rawValue);
                }
                else if (typeof(TValue) == typeof(byte))
                {
                    value = (TValue)(object)Convert.ToByte(rawValue);
                }
                else
                {
                    value = (TValue)rawValue;
                }

                return true;
            }
            catch
            {
                if (validationMessage != null)
                {
                    MessageBox.Show(validationMessage, validationTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }
        }

        // Overload for required values with automatic message
        public static bool TryGetRequiredValue<TValue>
            (
                this Control control,
                out TValue value,
                string fieldName
            )
        {
            return control.TryGetValue(
                out value,
                $"{fieldName} is required",
                "Validation");
        }

        #endregion
    }
}
