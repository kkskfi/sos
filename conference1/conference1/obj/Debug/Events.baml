   M S B A M L   `   `   `  ����F  Bconference1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:conference1conference1  5          conference1.Events   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:conference1     5        N� Title$  Events��5	      .��5      �  � 	Resources 5   
    	  MyCol �� Orientation �� StringFormat �� Path��%V  &	        �� 5      .R�5      �� $��1q�6   
���   �$��5,5,5,5q�6A   ._�5      �� $
��auto��6!   $
��auto��6.   $ 
Horizontal=�6<   .+�5      �� $��50��6=   $��50��6H   $��Right=�6T   ��6    �� *	PhotoPath+6r   �� 5      $ Vertical=�6%   $
��auto��6<   .+�5      �� $	��600��6E   $
��Left=�6Q   ��6(   �� *TitleEvents+6n   �� 5      $
��auto��6Y   $
��Left=�6f   ��6(   �� *Date+$ 
dd.MM.yyyy��6�   �� 5      $
��auto��6Q   $
��Left=�6^   ��6(   �� $ Direction1.Title�6{   5      5      5      5      5       � RowDefinitions 5   
   �� 5      $��1* �6   �� 5       $��8* �6   �� 5!      $��1* �6   .+�5$   
   �� -   5         ��Name$ 
Directions��5$      $��2��6(   $	��100��69   $��Center=�6E   $��Center=�6b   ��6}   �� ��5&      �� 5'      .R�5(      �� ��6    �� *Title+69   5)      5,      � 5.   
   $��0��6    � ColumnDefinitions 5/      �� 50      6%   �� 51      6%   �� 52      6%   54      &   System.Windows.Controls.DataGrid  57   
   -   5          Name$ 	eventGrid��57   /   	   AutoGenerateColumns	 .  $��1��6B   
   
IsReadOnly
 .    GridLinesVisibility$
 None=�6a      Columns 58      4  .System.Windows.Controls.DataGridTemplateColumn  59         CellTemplate# [  6+   ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6Q   5:      5<      � 5?   
   $��2��6    5@      �� 5A      6%   �� 5B      6%   �� 5C      6%   5E      � 5H   
   $��3��6    5I      �� 5J      $��* �6#   6/   .+�5M      �� -   5         ��Name$ authorizationButton��5M      $
��auto��62   $	��100��6@   $��0��6L   $��Авторизация��6\   6r   �� 5N      -   5        $ 
add_Button��5N      $
��auto��6)   $	��100��67   $��0��6C   $��Добавить��6S   $y�Hidden=�6f   6{   5O      5P      5Q      