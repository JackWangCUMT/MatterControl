﻿using MatterHackers.Agg.UI.Tests;
using MatterHackers.GuiAutomation;
using NUnit.Framework;
using System;

namespace MatterHackers.MatterControl.Tests.Automation
{
	[TestFixture, Category("MatterControl.UI.Automation"), RunInApplicationDomain]
	public class SliceSetingsTests
	{
		[Test, RequiresSTA, RunInApplicationDomain]
		public void RaftEnabledPassedToSliceEngine()
		{
			// Run a copy of MatterControl
			Action<AutomationTesterHarness> testToRun = (AutomationTesterHarness resultsHarness) =>
			{
				AutomationRunner testRunner = new AutomationRunner(MatterControlUtilities.DefaultTestImages);
				{
					MatterControlUtilities.PrepForTestRun(testRunner);

					MatterControlUtilities.SelectAndAddPrinter(testRunner, "Airwolf 3D", "HD");

					//Navigate to Local Library 
					testRunner.ClickByName("Library Tab");
					MatterControlUtilities.NavigateToFolder(testRunner, "Local Library Row Item Collection");
					testRunner.Wait(1);
					testRunner.ClickByName("Row Item Calibration - Box");
					testRunner.ClickByName("Row Item Calibration - Box Print Button");
					testRunner.Wait(1);

					testRunner.ClickByName("Layer View Tab");

					testRunner.ClickByName("Bread Crumb Button Home", 1);
					testRunner.ClickByName("SettingsAndControls", 1);
					testRunner.ClickByName("User Level Dropdown", 1);
					testRunner.ClickByName("Advanced Menu Item", 1);
					testRunner.ClickByName("Skirt and Raft Tab", 1);

					testRunner.ClickByName("Create Raft Checkbox", 1);
					testRunner.Wait(1.5);
					testRunner.ClickByName("Generate Gcode Button", 1);
					testRunner.Wait(5);

					//Call compare slice settings methode here
					resultsHarness.AddTestResult(MatterControlUtilities.CompareExpectedSliceSettingValueWithActualVaue("enableRaft", "True"));


					MatterControlUtilities.CloseMatterControl(testRunner);
				}
			};

			AutomationTesterHarness testHarness = MatterControlUtilities.RunTest(testToRun, overrideWidth: 1224, overrideHeight: 800);

			Assert.IsTrue(testHarness.AllTestsPassed);
			Assert.IsTrue(testHarness.TestCount == 1); // make sure we ran all our tests
		}

		//Stress Test check & uncheck 1000x
		[Test, RequiresSTA, RunInApplicationDomain, Ignore("Not Finished")]
		public void HasHeatedBedCheckUncheck()
		{
			// Run a copy of MatterControl
			Action<AutomationTesterHarness> testToRun = (AutomationTesterHarness resultsHarness) =>
			{
				AutomationRunner testRunner = new AutomationRunner(MatterControlUtilities.DefaultTestImages);
				{
					MatterControlUtilities.PrepForTestRun(testRunner);

					MatterControlUtilities.SelectAndAddPrinter(testRunner, "Airwolf 3D", "HD");

					//Navigate to Local Library 
					testRunner.ClickByName("SettingsAndControls");
					testRunner.Wait(1);
					testRunner.ClickByName("User Level Dropdown");
					testRunner.Wait(1);
					testRunner.ClickByName("Advanced Menu Item");
					testRunner.Wait(1);
					testRunner.ClickByName("Printer Tab");
					testRunner.Wait(1);

					testRunner.ClickByName("Features Tab");
					testRunner.Wait(2);

					for (int i = 0; i <= 1000; i++)
					{
						testRunner.ClickByName("Has Heated Bed Checkbox");
						testRunner.Wait(.5);
					}

					MatterControlUtilities.CloseMatterControl(testRunner);
				}
			};

			AutomationTesterHarness testHarness = MatterControlUtilities.RunTest(testToRun);

			Assert.IsTrue(testHarness.AllTestsPassed);
			Assert.IsTrue(testHarness.TestCount == 0); // make sure we ran all our tests
		}

		[Test, RequiresSTA, RunInApplicationDomain]
		public void HasHeatedBedCheckedHidesBedTemperatureOptions()
		{
			// Run a copy of MatterControl
			Action<AutomationTesterHarness> testToRun = (AutomationTesterHarness resultsHarness) =>
			{
				AutomationRunner testRunner = new AutomationRunner(MatterControlUtilities.DefaultTestImages);
				{
					MatterControlUtilities.PrepForTestRun(testRunner);

					MatterControlUtilities.SelectAndAddPrinter(testRunner, "Airwolf 3D", "HD");

					//Navigate to Settings Tab and make sure Bed Temp Text box is visible 
					testRunner.ClickByName("SettingsAndControls");
					testRunner.Wait(.5);
					testRunner.ClickByName("User Level Dropdown",1);
					testRunner.ClickByName("Advanced Menu Item", 1);
					testRunner.ClickByName("Filament Tab", 1);
					testRunner.ClickByName("Temperatures Tab", 1);
					resultsHarness.AddTestResult(testRunner.WaitForName("Bed Temperature Textbox", 2));

					//Uncheck Has Heated Bed checkbox and make sure Bed Temp Textbox is not visible
					testRunner.ClickByName("Printer Tab",1);
					testRunner.ClickByName("Features Tab", 1);
					testRunner.DragByName("Show Reset Connection Checkbox", 1, offset: new Agg.Point2D(-40, 0));
					testRunner.MoveToByName("Show Reset Connection Checkbox", 1, offset: new Agg.Point2D(0, 120));
					testRunner.Drop();
					testRunner.ClickByName("Has Heated Bed Checkbox", 1);
					testRunner.Wait(.5);
					testRunner.ClickByName("Filament Tab", 1);
					bool bedTemperatureTextBoxVisible = testRunner.WaitForName("Bed Temperature Textbox", 2);
					resultsHarness.AddTestResult(bedTemperatureTextBoxVisible == false);

					//Make sure Bed Temperature Options are not visible in printer controls
					testRunner.ClickByName("Controls Tab");
					bool bedTemperatureControlsWidget = testRunner.WaitForName("Bed Temperature Controls Widget", 2);
					resultsHarness.AddTestResult(bedTemperatureTextBoxVisible == false);

					MatterControlUtilities.CloseMatterControl(testRunner);
				}
			};

			AutomationTesterHarness testHarness = MatterControlUtilities.RunTest(testToRun);

			Assert.IsTrue(testHarness.AllTestsPassed);
			Assert.IsTrue(testHarness.TestCount == 3); // make sure we ran all our tests
		}
	}
}
