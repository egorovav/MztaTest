   M S B A M L   `   `   `  ����C  ?MztaTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:MztaTest.ViewModelMztaTest.ViewModel  5   	       MztaTest.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   2model clr-namespace:MztaTest.ViewModel     5   	     � Title$  COM Port Tester��5   	   $	��350��6!   $	��525��6.   .��5      � .+�5   
   �� $
��Left=�6   $	��Top=�60   $	��309��6H   $	��507��6U   .@�5      �� $��Settings��6   $��22��6(   $	��Top=�64   .��5	      � -   5         �Name$ SettingsGrid��5	   .   ���	#FFE5E5E5.+�5
      �� $��Ports��6   $
��Left=�6-   $��	10,10,0,0q�6H   $	��Top=�6[   $��75��6s   ��5      �� *GetPortListCommand+��68   �� �� 5      $
��Left=�6   $	��100��6:   $��
103,10,0,0q�6G   $	��Top=�6[   $	��100��6s   ��5      �� *
PortList+ �� SelectedValue 64   �� *
PortName+5      �� 5      $
��Left=�6    $��
218,17,0,0q�6;   $
��Wrap=�6O   $��	Baud Rate��6c   $	��Top=�6t   �� 5      $
��Left=�6   $��23��69   $��
216,43,0,0q�6E    �� TextWrapping$
 Wrap=�6Y   $	��Top=�6m   $��63��6�   ��5      �� *
BaudRate+ �� Mode$ TwoWay=�.��6E   5      5      5      �� 5      $��Exchange��6   .��5      � -   5        $ ExchangeGrid��5   .   ���	#FFE5E5E5 � ColumnDefinitions 5      �� 5      $	��10* �6+   �� 5      $
��157* �6+   .+�5      �� $
��Left=�6   $��23��69   $��0,10,0,0q�6E   $
 Wrap=�6W   $	��Top=�6k   $	��120��6�   $��2��5   P   ��6   �� *	Message+ �� UpdateSourceTrigger$ PropertyChanged=�6P   �� 5      $
��Send��6   $
��Left=�6,   $��	95,10,0,0q�6G   $	��Top=�6Z   $��59��6r   $��1��5   N   ��6   �� *SendCommand+��61   �� 6N   �� 5      $��
HEX Format��6   $
��Left=�64   $��	10,38,0,0q�6O   $	��Top=�6b   $��2��5    @    d� 	IsChecked 6   �� *IsInputDataInHexFormat+6@   �� 5!      $
��Left=�6   $	��193��6:   $��0,58,0,0q�6G   $	��Top=�6Y   $	��193��6q   $��2��5"   4   ��6   �� *
Exchange+5$      �� 5%      $��1��6    $
��Left=�60   $��
175,14,0,0q�6K   $
��Wrap=�6_   $	��Top=�6�   
���  ����6s   �� *ErrorString+6�   �� 5&      $��	Read data��6   $
��Left=�61   $��
10,256,0,0q�6L   $	��Top=�6`   $��75��6x   $��2��6�   ��5'      �� *ReadCommand+��6=   �� �� 5(      $
��Stop��6   $��1��6,   $
��Left=�6<   $��
70,256,0,0q�6W   $	��Top=�6k   $��75��6�   ��5)      �� *StopCommand+��6=   �� 5*      5+      5,      5-      5.      