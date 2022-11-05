using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ObjectExtensions
{
  public const string MIDDLE = " cannot be converted to type ";

  public static bool IsNull(this object obj) => obj == null;

  public static bool IsNotNull(this object obj) => !obj.IsNull();

  public static bool HasMethod(this object target, string methodName) => target.GetType().HasMethod(methodName);

  public static bool HasField(this object target, string fieldName) => target.GetType().HasField(fieldName);

  public static bool HasProperty(this object target, string propertyName) => target.GetType().HasProperty(propertyName);
}