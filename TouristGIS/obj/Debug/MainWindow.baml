   M S B A M L   `   `   `  ����E  ATouristGIS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:TouristGIS.ViewModelsTouristGIS.ViewModels  5   	       TouristGIS.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Z VEsri.ArcGISRuntime, Version=10.2.7.0, Culture=neutral, PublicKeyToken=8fc3cc631e44ad866esri+http://schemas.esri.com/arcgis/runtime/2013  5   	   :d2http://schemas.microsoft.com/expression/blend/2008  5   	   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	   /local#clr-namespace:TouristGIS.ViewModels  5   	   X TXceed.Wpf.Toolkit, Version=3.0.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  
MainWindow��5
   	    � WindowState$ 	Maximized=�6    � 	Resources 5      )   #TouristGIS.ViewModels.MainViewModel   MainViewModel��%   &	          5   
   6   5       � DataContext 5      �� 5   
    �� Source# [  6   .��5      �  5   
    �� 
TargetType �� BasedOn��%0   '	�       7d��� 5      " 	�6   8   6-   5      .+�5   
   )  #Esri.ArcGISRuntime.Controls.MapView  -   5          Name$ 	MyMapView��5   @   $��3��6   $y�Visible=�6+   $��0,-42,0,-0.2q�5   I   
   Map 5   S   �� *Map+*  $Esri.ArcGISRuntime.Controls.ViewBase	   Overlays	 5      5  /Esri.ArcGISRuntime.Controls.OverlayItemsControl  5      
   Items.
 5      �� -   5         ��Name$ FeatureOverlay��5      ���	#CCFFFFFF
���   �$��2q�6`    �� Padding$ 4q�5      $��Right=�6   $��Bottom=�60   $	��300��6K   $y�	Collapsed=�5      ._�5      � .+�5      �� $��Feature:��6(   $��14(�68   $
��Bold"�6F   �� 5      $��0,25,0,0q�6J   ��6'   �� *
Attributes+��5    "   �� 5!   &   .R�5"   *   ��  �� Orientation$ 
Horizontal=�65   .+�5#   .   �� $��75��68   $
��Bold"�6X   $��5,0,0,0q�6j   ��6C   �� *Key+6j   �� 5$   .   $	��175��68   ��6D   �� *Value+5%   +   5&   '   5(      5)      5*      5+      5-      �� 5/   
   $
��Left=�6   $	��Top=�6,   $��	20,20,0,0q�6D   $ 15q�6W   $	��300��6d   
�������
���   �$��1q�50   8   $	��575��6L    J� Effect 51      3  -System.Windows.Media.Effects.DropShadowEffect  52      ._�54      �� .+�55      �� $��Tourist GIS��6   $
��Wrap=�56   
   �� 57      $ 
Horizontal=�6   .+�58      �� -   5        $��	Add layer��58      $	��100��61   $��20��6=   $��0,10,0,0q�6I   6[   �� 59      -   5        $��Remove layer��59      $	��100��64   $��20��6@   $��5,10,0,0q�6L   6_   �� 5:      -   5        $	��⬆��5:      $��20��6)   $��20��64   $��5,10,0,0q�6@   6S   �� 5;      -   5        $	��⬇��5;      $��20��6)   $��20��64   $��5,10,0,0q�6@   6S   5<      �� 5=      $��0q�6   .+�5>      �� .��6   	Layers:6%   �� 5?      -   5         ��Name$ BasemapListBox��5?      $��0��66   $	��Top=�5A      $
��Left=�6.   $	��120��5B      $	��258��6#   $��
0,0,-0.4,0q�6a   ��5@      �� *LoadedLayers+��5C      �� 5D      .R�5E   "   �� $ 
Horizontal=�6-   .+�5F   &   �� -   5        $��5 5 0 0q�5F   Z    d� 	IsChecked 6/   �� *	Checked+ �� Mode$ TwoWay=�6k   �� 5G   &   ��6,   �� *Name+6G   5H   #   5I      5K      5L      �� 5M      $ 
Horizontal=�6   $��0,10,0,0q�66    6H   �� *LayerProperty+.+�5N      �� -	   5        $��ShowLabels_chk��5N      $��0,2,10,0q�65   $
 TrueY�5O   F   .��6X   Show labels6e   �� 5P      $��Select attribute:��6   $��0,1,5,0q�68   $��0q�6I   �� 5Q      -
   5        $��attributesComboBox��5Q      $��22��5R   V   $��77��6b    �� SelectedItem 5Q   9   �� *SelectedAttribute+��6e   �� *
Attributes+5R   b   5S      �� 5T      $ 
Horizontal=�6   $��0,10,0,0q�66    6H   �� *LayerProperty+.+�5U      �� $
��Left=�6   $��Layer display style:��67   $��0,1,5,0q�6V   $��0q�6g   �� 5V      -   5        $��displayStyleComboBox��5V      $��22��5W   X   $	��154��6d    5V   ;   ��  �� Path$ SelectedMarker���6h   �� $ AttributeMarkers�5W   d   5X      �� 5Y      $ 
Horizontal=�6   $��0,10,0,0q�66    6H   �� *LayerProperty+.+�5Z      �� $
��Left=�6   $��Layer display color:��67   $��0,1,5,0q�6V   $��0q�6g   #  Xceed.Wpf.Toolkit.ColorPicker  5[      -   5        $��ColorPicker��5[   '      DisplayColorAndName . $��22��5\   _   $	��151��6k      SelectedColor 5[   U   �� *SelectedColor+5\   k   5]      �� 5^      $ 
Horizontal=�6   $��0,10,0,0q�66   .+�5_      �� $��Select map style:��6   $��0,1,5,0q�68   $��0q�6I   �� 5`      -   5        $��22��5`   U   $	��165��6a   .@�5a      �� $
��Topo��6'    �� 
IsSelected . 
 � Tag$T Nhttp://services.arcgisonline.com/arcgis/rest/services/World_Topo_Map/MapServer��5b      �� 5c      $��Streets��6'   $V Phttp://services.arcgisonline.com/arcgis/rest/services/World_Street_Map/MapServer��5d      �� 5e      $��Imagery��6'   $S Mhttp://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer��5f      5g      5h      �� 5i      $��0,10,0,0q�6   .+�5j      �� $��4326 Coordinates��6    $
��Bold"�68   �� 5k      -   5         ��Name$ screenX��5k       $
��X: 0��61   �� 5l      -   5        $ screenY��5l       $
��Y: 0��61   �� 5m      $��GK Coordinates��6    $
��Bold"�66   �� 5n      -   5        $
 mapX��5n       $
��X: 0��6.   �� 5o      -   5        $
 mapY��5o       $
��Y: 0��6.   5p      �� 5q      -   5        $ txtInfo��5q      $��12(�6-   $y�	Collapsed=�6;   $��0,10,0,0q�6R   �� 5r      $�
Shapefile:��6   $��14(�6   $
��Bold"�6+   �� 5s      �� 5t      $�Name: ��6   $
��Bold"�6    �� 5u      �6   �� *ID+$ OneWay=��� 5v      �� 5w      $�Path: ��6   $
��Bold"�6    �� 5x      �6   �� *DisplayName+$ OneWay=��� 5y      �� 5z      $�Spatial Reference: ��6   $
��Bold"�6&    �� 5{      �6   �� *$"FeatureTable.SpatialReference.Wkid+$ OneWay=��� 5|      �� 5}      $�Geometry Type: ��6   $
��Bold"�6"    �� 5~      �6   �� *FeatureTable.GeometryType+$ OneWay=��� 5      �� 5�      $�
Features: ��6   $
��Bold"�6    �� 5�      �6   �� *FeatureTable.RowCount+$ OneWay=�5�      5�      5�      �� 5�   
   $��Right=�6   $	��Top=�6-   $��	0,20,20,0q�6E   $ 15q�6X   $��80��6e   
�������
���   �$��1q�5�   8   $	��465��6L    5�        5�      ._�5�      �� .+�5�      	� -   5         � ToolTip$ 	Rectangle��5�      $��45��62   $��45��6=   $��0,0,0,10q�6I   ���	#FFEEF5FD.��5�      �� $ ��Images\SelectRectangle.png��6   6B   5�      	� 5�      -   5        $ Polygon��5�      $��45��60   $��45��6;   $��0,0,0,10q�6G   ���	#FFEEF5FD.��5�      �� $��Images\SelectPolygon.png��6   6@   5�      	� 5�      -   5        $��45��5�      $��45��6)   $��0,0,0,10q�65   ���	#FFEEF5FD.��5�      �� $��Images\SelectFeature.png��6   6@   5�      	� 5�      -   5        $��45��5�      $��45��6)   $��0,0,0,10q�65   ���	#FFEEF5FD.��5�      �� $��Images\DeselectAll.png��6   6>   5�      �� 5�      -   5        $��45��5�      $��45��6$   $��0,0,0,10q�60   ���	#FFEEF5FD.��5�      �� $ ��Images\AttributeFilter.png��6   6B   5�      	� 5�      -   5        $��45��5�      $��45��6)   $��0,0,0,10q�65   ���	#FFEEF5FD.��5�      �� $ ��Images\MeasureDistance.png��6   6B   5�      �� 5�      -   5        $��45��5�      $��45��6$   $��0,0,0,10q�60   ���	#FFEEF5FD.��5�      �� $��Images\SpatialFilter.png��6   6@   5�      �� 5�      -   5        $��45��5�      $��45��6$   $��0,0,0,10q�60   ���	#FFEEF5FD.��5�      �� $��Images\DeleteSelected.png��6   6A   5�      5�      5�      5�      5�      