   M S B A M L   `   `   `  ����F  BAvtoservice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Avtoservice.PagesAvtoservice.Pages  5           Avtoservice.Pages.Services   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:Avtoservice.Pages     5        N� Title$  Services��5	      .��5      �  � 	Resources 5   
    	  MyCol �� Orientation ��Name��%�  &	        �� 5      .R�5      �� $��1q�6   
���   �$��5,5,5,5q�6A   ._�5      �� $
��auto��6!   $
��auto��6.   $ 
Horizontal=�6<   .+�5      �� $ Vertical=�6%   $
��auto��6<   .+�5      �� $
��auto��6?   $
��Left=�6L   ��6(   �� *Title+6i   �� 5      $
 sale��6>   $	��100��6L   $
��Left=�6X   ��6(   �� *Cost+6u   �� 5      $	��100��6K   $
��Left=�6W   ��6(   �� *DurationInSeconds+6t   5      5      5      5      5       � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��8* �6   �� 5       $��1* �6   �� 5!      $��1* �6   .+�5#   
   � $��0��6    � ColumnDefinitions 5$      �� 5%      6%   �� 5&      6%   �� 5'      6%   �� 5(      6%   .+�5*      �� -   5        $��0��5*      6J   �� 5+      -   5        $��1��5+      .@�5,      �� 
 � Tag$ 0��6   .��5-      �� $!��Без сортировки��6    69   5.      �� 5/      $ 1��6   .��50      �� $2��,По возрастанию названия��6    6B   51      �� 52      $ 2��6   .��53      �� $,��&По убыванию названия��6    6>   54      56      �� 57      -   5         ��Name$
 Type��57      $��2��6%   ��66   �� ��59      �� 5:      .R�5;      �� ��6$   �� *Title+6=   5<      5?      �� 5@      -   5        $ 	cmb_count��5@   '   $��3��6   .@�5A      �� $ 0��6   .��5B      �� $��10��6    6-   5C      �� 5D      $ 1��6   .��5E      �� $��50��6    6-   5F      �� 5G      $ 2��6   .��5H      �� $	��200��6    6-   5I      5J      5N      &   System.Windows.Controls.DataGrid  5P   
   -   5          Name$ serviceGrid��5P   /   	   AutoGenerateColumns	 .  $��1��6D   
   
IsReadOnly
 .    GridLinesVisibility$
 None=�6c      Columns 5Q      4  .System.Windows.Controls.DataGridTemplateColumn  5R         CellTemplate# [  6)   ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6O   5S      5U      � 5V   
   $��2��6    5X      �� 5Y      6%   �� 5Z      6%   �� 5[      6%   5^      � 5_   
   $��3��6    5`      �� 5a      $��* �6#   6/   �� 5b      $��* �6#   6/   �� 5c      $��* �6#   6/   �� 5d      $��* �6#   6/   .+�5f      �� -   5         ��Name$ 	addButton��5f      $
��auto��6(   $
��auto��66   $��2��6C   $��Добавить��6S   6f   5h      �� 5i   
   $��2��6   $��2��6&   $ 
Horizontal=�64   $��Stretch=�6M   .+�5j      �� $��Всего: ��6   $
��Left=�6'   $��Center=�6B   6_   �� 5k      -   5        $
 full��5k      $�� ��6&   $��Right=�6.   $��Center=�6J   5l      5m      �� 5p   
   $��2��6   $��2��6&   $ 
Horizontal=�63   $��	300 0 0 0q�6L   $��Stretch=�6_   $
��auto��6}   .+�5q      �� -   5        $
 back��5q      $��B��6#   {�.  $��30��6V   $��20��6b   6o   �� 5r      -	   5         ��Name$ pagin��5r      $��2��6(   $��2��68   $ 
Horizontal=�6E   $��Stretch=�6^   $
��auto��6|   5s      �� 5t      -
   5        $ forward��5t      $��F��6&   $��30��6H   $��20��6T   6a   5u      5{      5}      