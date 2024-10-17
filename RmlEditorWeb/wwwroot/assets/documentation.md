
# Receipt Markup Language (RML) Documentation

RML is a custom XML-based language designed to define the layout and content of a printed receipt. It provides detailed control over the structure and styling of receipt elements, enabling dynamic data binding and flexible layout configurations. This documentation serves as a guide to understanding and utilizing RML for generating and rendering printed receipts dynamically.



[Documentation](https://linktodocumentation)



[Overview](#overview)
[Document Structure](#document-structure)
[Elements](#elements)
 - [Receipt](#receipt)
 - [DataContext](#datacontext)
 - [Dictionary](#dictionary)
 - [Pair](#pair)
 - [Resources](#resources)
 - [DataSource](#datasource)
 - [Body](#body)
 - [Row](#row)
 - [Border](#border)
 - [Image](#image)
 - [TextBlock](#textblock)
 - [Repeater](#repeater)
 - [Barcode](#barcode)
[Attributes](#attributes)
 - [Width](#width)
 - [Margin](#margin)
 - [FontSize](#fontsize)
 - [HorizontalAlignment](#horizontalalignment)
[Styling](#styling)
 - [Text Styling](#text-styling)
 - [Background](#background)
[Data Binding](#data-binding)
[Examples](#examples)
[Common Use Cases](#common-use-cases)


---

## Overview

RML is created to provide a quick and reliable way to create and render standard 82mm printer receipts dynamically. It allows developers to generate receipts and tickets with complex layouts and dynamic content.




### Basic Receipt Structure
Example Receipt:

```
<Receipt>
  <DataContext>
    <Dictionary>
      <Pair Key="DrawNumber" Value="4564" />
      <Pair Key="DrawDate" Value="JAN 15, 2024" />
      <!-- Additional key-value pairs -->
    </Dictionary>
  </DataContext>

  <Resources>
    <DataSource Name="panels">
        <Dictionary rowIndex="A" numbers="0 6 9" playType="STRAIGHT" playAmount="$0.50" />
        <Dictionary rowIndex="B" numbers="3 7 2" playType="STRAIGHT" playAmount="$0.50" />
        <Dictionary rowIndex="C" numbers="1 4 9" playType="STRAIGHT" playAmount="$1.00" />
        <Dictionary rowIndex="D" numbers="3 6 4" playType="STRAIGHT" playAmount="$0.50" />
    </DataSource>
  </Resources>

  <Body Width="500px" Background="FFFFFF">
    <Row Margin="5,20,5,0">
      <TextBlock FontSize="26" Width="*" HorizontalAlignment="Center" Content="{DrawNumber}" />
    </Row>
    <!-- Additional rows and elements -->
  
    <Repeater DataSource="panels">
        <Row Margin="10">
            <TextBlock Width="*" HorizontalAlignment="Center">
            <Run FontWeight="Bold" Content="{rowIndex}" />
            <Run Content=" " />
            <Run Content="{numbers}" />
            <Run Content=" " />
            <Run Content="{playAmount}" />
            </TextBlock>
        </Row>
    </Repeater>
  
  </Body>
</Receipt>
```
This is a basic receipt structure that includes a DataContext for data binding, Resources for a repeater and a Body with a single Row displaying the DrawNumber.








## Document Structure

The RML document is structured hierarchically, starting with the root `<Receipt>` element. It contains the following primary sections:

**DataContext**: Defines data bindings for dynamic content.
**Resources**: Contains reusable resources like data sources.
**Body**: Defines the layout and elements to be displayed on the receipt.

## Elements

### `<Receipt>`
The root element of the RML document that encapsulates the entire receipt structure.

### `<DataContext>`
Defines the context in which data is used within the receipt. Contains a `<Dictionary>` element that stores key-value pairs for data binding.

```
<DataContext>
  <Dictionary>
    <Pair Key="DrawNumber" Value="4564" />
    <Pair Key="DrawDate" Value="JAN 15, 2024" />
    <!-- Additional key-value pairs -->
  </Dictionary>
</DataContext>
```


## Data Binding

Data binding in RML allows for dynamic content to be displayed based on keys defined within the `<DataContext>` element. The `Dictionary` element inside `DataContext` stores these key-value pairs, which can be referenced throughout the receipt layout.

### Example Usage

```
<TextBlock Content="{DrawNumber}" />
```


## Resources

Resources in RML are reusable components, such as data sources, that can be defined once and used multiple times within the receipt layout.

### Example Data Source

```
<Resources>
  <DataSource Name="panels">
    <Dictionary rowIndex="A" numbers="0 6 9" playType="STRAIGHT" playAmount="$0.50" />
    <Dictionary rowIndex="B" numbers="3 7 2" playType="STRAIGHT" playAmount="$0.50" />
    <Dictionary rowIndex="C" numbers="1 4 9" playType="STRAIGHT" playAmount="$1.00" />
    <Dictionary rowIndex="D" numbers="3 6 4" playType="STRAIGHT" playAmount="$0.50" />
  </DataSource>
</Resources>
```

This DataSource named "panels" contains multiple Dictionary entries representing different data rows. Each Dictionary entry can be used in elements like <Repeater> to dynamically create repeated content on the receipt.



## Repeater Element

The `<Repeater>` element is used to repeat a set of elements for each item in a specified data source. This is particularly useful for creating lists of items, such as purchased products or lottery numbers.

### Example Repeater

```
<Repeater DataSource="panels">
  <Row Margin="10">
    <TextBlock Width="*" HorizontalAlignment="Center">
      <Run FontWeight="Bold" Content="{rowIndex}" />
      <Run Content=" " />
      <Run Content="{numbers}" />
      <Run Content=" " />
      <Run Content="{playAmount}" />
    </TextBlock>
  </Row>
</Repeater>
```

In this example, the `Repeater` element iterates over the `DataSource` named `"panels"`. For each `Dictionary` entry in the data source, a new `Row` element is created with a `TextBlock` displaying the `rowIndex`, `numbers`, and `playAmount` values.



## Barcode Element

The `<Barcode>` element is used to generate barcodes in the receipt layout. It can be configured to support various barcode formats and content.

### Example Barcode

```
<Barcode Format="Pdf417" Content="{DrawNumber}" />
```

This `Barcode` element will generate a `Pdf417` or various other barcode/QrCode using the value bound to `{DrawNumber}`, which in this case is `"4564"`.


### Width

Defines the width of an element. The hierarchy for width calculation is as follows:

Exact Pixel Width (px): Takes up the specified amount of space first.
Auto Width: Adjusts based on content size.
Proportional Width (*): Divides remaining space among elements using * or n* widths.
Example:


```
<TextBlock Width="100px" Content="{DrawNumber}" />
```

In this example, the TextBlock will occupy exactly 100 pixels of width.


### Margin

Margin can be on the `Row Element`

The format will be in this order `Left`, `Top`, `Right`, `Bottom`

Example:


```
<Row Margin="10, 5, 10, 5">
  <TextBlock Content="{DrawNumber}" />
</Row>
```
This Row element has a margin of 10 pixels on the left and right, and 5 pixels on the top and bottom.

`or`


```
<Row Margin="`0`">
  <TextBlock Content="{DrawNumber}" />
</Row>
```
This Row element has a margin of 10 pixels but since its the only value it will be applied to Left, Right, Top, and bottom.



### FontSize

Defines the size of the text in a <TextBlock> element.

Example:

```
<TextBlock FontSize="20" Content="{DrawNumber}" />
```
This TextBlock will render the text with a font size of 20.

### HorizontalAlignment
Aligns an element horizontally within its container.

Values: `Left`, `Center`, `Right`, `Stretch`

Example:

```
<TextBlock HorizontalAlignment="Center" Content="{DrawNumber}" />
```

This TextBlock will be centered horizontally within its container.


### Styling

Text elements like <TextBlock> and <Run> support various styling attributes such as `FontSize`, `FontWeight`, and `TextDecorations`.

Example:

```
<Run FontSize="16" FontWeight="Bold" Content="{DrawDate}" />
```
This Run element will render bold text with a font size of 16.

### Background

The Background attribute defines the background color of an element using a hexadecimal color code and is only for the `Body` tag.

Example:

```
<Body Background="#FFFFFF">
  <!-- Body content -->
</Body>
```
This Body element will have a white background.


