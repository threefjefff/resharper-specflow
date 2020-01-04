//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Windows.Markup.XmlnsDefinitionAttribute("urn:shemas-jetbrains-com:ui-application-icons-unnamed", "ReSharperPlugin.SpecflowRiderPlugin")]
[assembly: global::JetBrains.UI.Icons.CompiledIcons.CompiledIconsPackAttribute(IconPackResourceIdentification="ReSharperPlugin.SpecflowRiderPlugin;component/Icons/ThemedIcons.Unnamed.Generated" +
	".Xaml", IconNames=new string[] {
		"Gherkin"})]

namespace ReSharperPlugin.SpecflowRiderPlugin
{
	
	
	/// <summary>
	///  <para>
	///    <para>Autogenerated identifier classes and identifier objects to Themed Icons registered with <see cref="JetBrains.UI.Icons.IThemedIconManager" />.</para>
	///    <para>Identifier classes should be used in attributes, XAML, or generic parameters. Where an <see cref="JetBrains.UI.Icons.IconId" /> value is expected, use the identifier object in the <c>Id</c> field of the identifier class.</para>
	///  </para>
	///</summary>
	///<remarks>
	///  <para>This code was compile-time generated to support Themed Icons in the JetBrains application.</para>
	///  <para>It has two primary goals: load the icons of this assembly to be registered with <see cref="JetBrains.UI.Icons.IThemedIconManager" /> so that they were WPF-accessible and theme-sensitive; and emit early-bound accessors for referencing icons in codebehind in a compile-time-validated manner.</para>
	///  <h1>XAML</h1>
	///  <para>For performance reasons, the icons are not individually exposed with application resources. There is a custom markup extension to bind an image source in markup.</para>
	///  <para>To use an icon from XAML, set an <see cref="System.Windows.Media.ImageSource" /> property to the <see cref="JetBrains.UI.Icons.ThemedIconExtension" /> markup extension, which takes an icon identifier class (nested in <see cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons" /> class) as a parameter.</para>
	///  <para>Example:</para>
	///  <code>&lt;Image Source="{icons:ThemedIcon myres:UnnamedThemedIcons+Gherkin}" /&gt;</code>
	///  <h1>Attributes</h1>
	///  <para>Sometimes you have to reference an icon from a type attriute when you're defining objects in code. Typical examples are Options pages and Tool Windows.</para>
	///  <para>To avoid the use of string IDs which are not validated very well, we've emitted identifier classes to be used with <c>typeof()</c> expression, one per each icon. Use the attribute overload which takes a <see cref="System.Type" /> for an image, and choose your icon class from nested classes in the <see cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons" /> class.</para>
	///  <para>Example:</para>
	///  <code>[Item(Name="Sample", Icon=typeof(UnnamedThemedIcons.Gherkin))]</code>
	///  <h1>CodeBehind</h1>
	///  <para>In codebehind, we have two distinct tasks: (a) specify some icon in the APIs and (b) render icon images onscreen.</para>
	///  <para>On the APIs stage you should only manipulate icon identifier objects (of type <see cref="JetBrains.UI.Icons.IconId" />, statically defined in <see cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons" /> in <c>Id</c> fields). Icon identifier classes (nested in <see cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons" />) should be turned into icon identifier objects as early as possible. Rendering is about getting an <see cref="System.Windows.Media.ImageSource" /> to assign to WPF, or <see cref="System.Drawing.Bitmap" /> to use with GDI+ / Windows Forms.</para>
	///  <para>You should turn an identifier object into a rendered image as late as possible. The identifier is static and lightweight and does not depend on the current theme, while the image is themed and has to be loaded or generated/rasterized. You need an <see cref="JetBrains.UI.Icons.IThemedIconManager" /> instance to get the image out of an icon identifier object. Once you got the image, you should take care to change it with theme changes — either by using a live image property, or by listening to the theme change event. See <see cref="JetBrains.UI.Icons.IThemedIconManager" /> and its extensions for the related facilities.</para>
	///  <para>Example:</para>
	///  <code>// Getting IconId identifier object to use with APIs
	///IconId iconid = UnnamedThemedIcons.Gherkin.Id;</code>
	///  <code>// Getting IconId out of an Icon Identifier Class type
	///IconId iconid = CompiledIconClassAttribute.TryGetCompiledIconClassId(typeof(UnnamedThemedIcons.Gherkin), OnError.Throw);</code>
	///  <code>// Getting image for screen rendering by IconId
	///themediconmanager.Icons[icnoid]</code>
	///  <code>// Getting image for screen rendering by Icon Identifier Class
	///themediconmanager.GetIcon&lt;UnnamedThemedIcons.Gherkin&gt;()</code>
	///  <h1>Icons Origin</h1>
	///  <para>This code was generated by a pre-compile build task from a set of input files which are XAML files adhering to a certain convention, as convenient for exporting them from the Illustrator workspace, plus separate PNG files with raster icons. In the projects, these files are designated with <c>ThemedIconsXamlV3</c> and <c>ThemedIconPng</c> build actions and do not themselves get into the output assembly. All of such files are processed, vector images for different themes of the same icon are split and combined into the single list of icons in this assembly. This list is then written into the genearted XAML file (compiled into BAML within assembly), and serves as the source for this generated code.</para>
	///</remarks>
	public sealed class UnnamedThemedIcons
	{
		
		/// <summary>
		///  <para>
		///    <para>Autogenerated identifier class for the Gherkin Themed Icon.</para>
		///    <para>Identifier classes should be used in attributes, XAML, or generic parameters. Where an <see cref="JetBrains.UI.Icons.IconId" /> value is expected, use the identifier object in the <see cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons.Gherkin.Id" /> field of the identifier class.</para>
		///  </para>
		///</summary>
		///<remarks>
		///  <para>For details on Themed Icons and their use, see Remarks on <see cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons" /> class.</para>
		///</remarks>
		///<seealso cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons" />
		///<example>
		///  <code>&lt;Image Source="{icons:ThemedIcon myres:UnnamedThemedIcons+Gherkin}" /&gt;        &lt;!-- XAML --&gt;</code>
		///</example>
		///<example>
		///  <code>[Item(Name="Sample", Icon=typeof(UnnamedThemedIcons.Gherkin))]        // C# Type attribute</code>
		///</example>
		///<example>
		///  <code>IconId iconid = UnnamedThemedIcons.Gherkin.Id;        // IconId identifier object</code>
		///</example>
		///<example>
		///  <code>themediconmanager.GetIcon&lt;UnnamedThemedIcons.Gherkin&gt;()        // Icon image for rendering</code>
		///</example>
		[global::JetBrains.UI.Icons.CompiledIcons.CompiledIconClassAttribute("ReSharperPlugin.SpecflowRiderPlugin;component/Icons/ThemedIcons.Unnamed.Generated" +
			".Xaml", 0, "Gherkin")]
		public sealed class Gherkin : global::JetBrains.UI.Icons.CompiledIcons.CompiledIconClass
		{
			
			/// <summary>
			///  <para>
			///    <para>Autogenerated identifier object for the Gherkin Themed Icon.</para>
			///    <para>Identifier objects should be used where an <see cref="JetBrains.UI.Icons.IconId" /> value is expected. In attributes, XAML, or generic parameters use the containing <see cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons.Gherkin" /> identifier class.</para>
			///  </para>
			///</summary>
			///<remarks>
			///  <para>For details on Themed Icons and their use, see Remarks on <see cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons" /> class.</para>
			///</remarks>
			///<seealso cref="ReSharperPlugin.SpecflowRiderPlugin.UnnamedThemedIcons" />
			///<example>
			///  <code>&lt;Image Source="{icons:ThemedIcon myres:UnnamedThemedIcons+Gherkin}" /&gt;        &lt;!-- XAML --&gt;</code>
			///</example>
			///<example>
			///  <code>[Item(Name="Sample", Icon=typeof(UnnamedThemedIcons.Gherkin))]        // C# Type attribute</code>
			///</example>
			///<example>
			///  <code>IconId iconid = UnnamedThemedIcons.Gherkin.Id;        // IconId identifier object</code>
			///</example>
			///<example>
			///  <code>themediconmanager.GetIcon&lt;UnnamedThemedIcons.Gherkin&gt;()        // Icon image for rendering</code>
			///</example>
			public static global::JetBrains.UI.Icons.IconId Id = new global::JetBrains.UI.Icons.CompiledIcons.CompiledIconId("ReSharperPlugin.SpecflowRiderPlugin;component/Icons/ThemedIcons.Unnamed.Generated" +
					".Xaml", 0, "Gherkin");
		}
	}
}


public sealed class RenderedIcons_ByPackResourceNameHash_6230B6F38B493A1C5B9053BF4A32B2D8
{
	
	public static byte[] Gherkin____png__x1 = new byte[] {
			137,
			80,
			78,
			71,
			13,
			10,
			26,
			10,
			0,
			0,
			0,
			13,
			73,
			72,
			68,
			82,
			0,
			0,
			0,
			171,
			0,
			0,
			0,
			171,
			8,
			6,
			0,
			0,
			0,
			25,
			232,
			108,
			25,
			0,
			0,
			0,
			1,
			115,
			82,
			71,
			66,
			0,
			174,
			206,
			28,
			233,
			0,
			0,
			0,
			4,
			103,
			65,
			77,
			65,
			0,
			0,
			177,
			143,
			11,
			252,
			97,
			5,
			0,
			0,
			0,
			9,
			112,
			72,
			89,
			115,
			0,
			0,
			14,
			195,
			0,
			0,
			14,
			195,
			1,
			199,
			111,
			168,
			100,
			0,
			0,
			0,
			134,
			73,
			68,
			65,
			84,
			120,
			94,
			237,
			193,
			1,
			1,
			0,
			0,
			0,
			128,
			144,
			254,
			175,
			238,
			8,
			2,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			128,
			26,
			201,
			158,
			0,
			1,
			205,
			132,
			45,
			115,
			0,
			0,
			0,
			0,
			73,
			69,
			78,
			68,
			174,
			66,
			96,
			130};
	
	public static byte[] Gherkin____png__x2 = new byte[] {
			137,
			80,
			78,
			71,
			13,
			10,
			26,
			10,
			0,
			0,
			0,
			13,
			73,
			72,
			68,
			82,
			0,
			0,
			1,
			86,
			0,
			0,
			1,
			86,
			8,
			6,
			0,
			0,
			0,
			50,
			175,
			181,
			131,
			0,
			0,
			0,
			1,
			115,
			82,
			71,
			66,
			0,
			174,
			206,
			28,
			233,
			0,
			0,
			0,
			4,
			103,
			65,
			77,
			65,
			0,
			0,
			177,
			143,
			11,
			252,
			97,
			5,
			0,
			0,
			0,
			9,
			112,
			72,
			89,
			115,
			0,
			0,
			14,
			195,
			0,
			0,
			14,
			195,
			1,
			199,
			111,
			168,
			100,
			0,
			0,
			1,
			221,
			73,
			68,
			65,
			84,
			120,
			94,
			237,
			193,
			1,
			13,
			0,
			0,
			0,
			194,
			160,
			247,
			79,
			109,
			14,
			55,
			32,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			56,
			85,
			3,
			37,
			79,
			0,
			1,
			31,
			126,
			133,
			104,
			0,
			0,
			0,
			0,
			73,
			69,
			78,
			68,
			174,
			66,
			96,
			130};
}
