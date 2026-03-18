import { test, expect } from '@playwright/test'

const BASE_URL = process.env.APP_URL || 'http://localhost:5000'

test.describe('Blood Pressure Calculator E2E Tests', () => {
  test('Page loads successfully', async ({ page }) => {
    await page.goto(BASE_URL)
    await expect(page).toHaveTitle(/BP/)
  })

  test('High blood pressure categorised correctly', async ({ page }) => {
    await page.goto(BASE_URL)
    await page.fill('input[name="BP.Systolic"]', '150')
    await page.fill('input[name="BP.Diastolic"]', '95')
    await page.click('input[type="submit"]')
    await expect(page.locator('body')).toContainText('High Blood Pressure')
  })

  test('Ideal blood pressure categorised correctly', async ({ page }) => {
    await page.goto(BASE_URL)
    await page.fill('input[name="BP.Systolic"]', '110')
    await page.fill('input[name="BP.Diastolic"]', '70')
    await page.click('input[type="submit"]')
    await expect(page.locator('body')).toContainText('Ideal Blood Pressure')
  })

  test('Low blood pressure categorised correctly', async ({ page }) => {
    await page.goto(BASE_URL)
    await page.fill('input[name="BP.Systolic"]', '85')
    await page.fill('input[name="BP.Diastolic"]', '55')
    await page.click('input[type="submit"]')
    await expect(page.locator('body')).toContainText('Low Blood Pressure')
  })

  test('Pre-high blood pressure categorised correctly', async ({ page }) => {
    await page.goto(BASE_URL)
    await page.fill('input[name="BP.Systolic"]', '130')
    await page.fill('input[name="BP.Diastolic"]', '85')
    await page.click('input[type="submit"]')
    await expect(page.locator('body')).toContainText('Pre-High Blood Pressure')
  })

  test('Recommendation shown alongside category', async ({ page }) => {
    await page.goto(BASE_URL)
    await page.fill('input[name="BP.Systolic"]', '150')
    await page.fill('input[name="BP.Diastolic"]', '95')
    await page.click('input[type="submit"]')
    await expect(page.locator('body')).toContainText('Recommendation')
  })

  test('Invalid input shows validation error', async ({ page }) => {
    await page.goto(BASE_URL)
    await page.fill('input[name="BP.Systolic"]', '80')
    await page.fill('input[name="BP.Diastolic"]', '90')
    await page.click('input[type="submit"]')
    await expect(page.locator('body')).toContainText('must be greater')
  })
  test('Out of range systolic shows error', async ({ page }) => {
    await page.goto(BASE_URL)
    await page.fill('input[name="BP.Systolic"]', '200')
    await page.fill('input[name="BP.Diastolic"]', '80')
    await page.click('input[type="submit"]')
    await expect(page.locator('span.text-danger').first()).toBeVisible()
  })
})
