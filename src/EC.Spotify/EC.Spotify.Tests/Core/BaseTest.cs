using System;
using System.Collections.Generic;
using System.Text;

namespace EC.Spotify.Tests.Core;

[TestClass]
public abstract class BaseTest
{
    protected virtual T? LoadSystemUnderTest<T>() => Initializer.Resolve<T>();
}
