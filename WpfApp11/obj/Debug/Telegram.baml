   M S B A M L   `   `   `  ����C  ?Telegram, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:WpfApp11WpfApp11  5   	       WpfApp11.Telegram   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:WpfApp11     5   	     � Title$  Telegram��5   	   $	��847��6   $	��789��6'   
���   �.��5	      �  � RowDefinitions 5   
   �� 5      $	��55* �6   �� 5      $
��715* �6   �� 5      $	��46* �6    � ColumnDefinitions 5   
   �� 5      $
��240* �6   �� 5      $
��541* �6   .+�5   
   �� -   5         ��Name$ searchGroup��5      $
��Left=�6'   $��25��6B   $��	70,19,0,0q�6N    �� TextWrapping$
 Wrap=�6a   $��Search��6u   $	��Top=�6�   $	��155��6�   ���	#FFEEE4E4���	#FFDFE2EA$��16(�6�   ���	#FF8B80806�   �� 5   
   -   5         ��Name$ listbox��5      $
��Left=�6#   $	��757��6>   $��1��6K   $	��Top=�6X   $	��240��6p   $��2��6|   �� 5   
   -   5         ��Name$ infoPage��5   T   $��0��6   $��0��6!   $��	0,0,175,0q�6.   
������� � Cursor$
 Hand��6f   $	��≡��6t    �� 	IsDefault . $��36(�6�   ���	#FFC5C5C56�   �� 5   
   -   5        $ chatBox��5      $
��Left=�6#   $��Center=�6>   $��46��6`   $��55,0,0,0q�6l   $��2��6~   $
 Wrap=�6�   $	��371��6�   $��14(�6�   	 �� SelectionBrush	 �	#FF00A2FF$��Bottom=�6�   $��1��6�   �� 5   
   -   5        
 ��Name$
 chatPage��5      $
��Left=�6%   $	��715��6@   $	��Top=�6M   $	��541��6e   $��1��6q   $��1��6~   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � |� �� Value .  62   5      5!       � 5#   
   -   5        $��LoadFileFrame��5#      $��60��6%   $	��260��61   $��1��6=   $��1��6J   $��281,655,0,0q�6Z   �� 5$   
   ���	#FFB0B0B0$��1q�6)   $
��Left=�6=   $��55��6X   $	��Top=�6d   $	��546��6|   $��1��6�   $��0,0,-5,0q�6�   �� 5%   
   ���	#FFB0B0B0$��1q�6)   $
��Left=�6=   $	��816��6X   $	��Top=�6e   $	��255��6}   $��3��6�   $��	-15,0,0,0q�6�   �� 5'   
   -   5        $ 
fileButton��5'      $
 Hand��6   $
��💾��63   $��1��6@   $
��Left=�6P   $��2��6k   $	��Top=�6x   $��55��6�   $��46��6�   
����������	#FFB0B0B06�   �� 5(   
   -   5        $ 
sendButton��5(      $
 Hand��6   $	��⯈��63   $��1��6?   $
��Left=�6O   $��2��6j   $	��Top=�6w   $��55��6�   $��46��6�   $��	431,0,0,0q�6�   
�������$��36(�6�   
����������	#FFCDCDCD6�   �� 5)   
   -	   5        $ voiceButton��5)      $
 Hand��6   $�� ��64   $��1��6?   $
��Left=�6O   $��2��6j   $	��Top=�6w   $��55��6�   $��46��6�   $��	486,0,0,0q�6�   
�������6�   �� 5*   
   -
   5         ��Name$ 	groupName��5*      $��1��6#   $
��Left=�63   $��	17,10,0,0q�6N   $	��Top=�6a   �� 5+   
   -   5        $ joinGroupButton��5+   4   $
 Hand��6   $��
Join Group��6   $
��Left=�6M   $��2��6h   $	��Top=�6u   $	��240��6�   $��46��6�   ���	#FF0083DA
�������$��24(�6�    � 5.   
   -   5          �Name$ 	mainFrame��5.      $y�Hidden=�6#   $
��Left=�67   $	��816��6R   $	��Top=�6_   $	��786��6w   ���	#59000000$��3��6�   $��2��6�   $��	0,0,-35,0q�6�    � 5/   
   -   5        $ userInfoFrame��5/      $
��Left=�6'   $	��816��6B   $	��Top=�6O   $	��240��6g   $��3��6s    � 50   
   -   5        $ listGroupFrame��50      $��1��6(   $
��Left=�68   $	��375��6S   $��1��6`   $	��Top=�6m   $	��320��6�   $��	0,170,0,0q�6�    � 51   
   -   5        $ createGroupFrame��51      $��1��6*   $
��Left=�6:   $	��320��6U   $��1��6b   $	��Top=�6o   $	��320��6�   $��	0,170,0,0q�6�    � 52   
   -   5        $ showContactsFrame��52      $��1��6+   $
��Left=�6;   $	��375��6V   $��1��6c   $	��Top=�6p   $	��320��6�   $��	0,170,0,0q�6�   58      59      